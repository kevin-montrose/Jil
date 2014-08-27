using Jil.Common;
using Sigil;
using Sigil.NonGeneric;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Deserialize
{
    internal static class NameAutomata<T>
    {
        public static class Helper
        {
            public static T Consume(TextReader tr, int ch, T errorValue)
            {
                while (ch != '\"' && ch != -1)
                    ch = tr.Read();
                return errorValue;
            }

            // TODO: This is duplicated around the place, so should consolidate
            public static char ReadHexQuad(TextReader reader)
            {
                int unescaped;
                int c;

                c = reader.Read();
                if (c >= '0' && c <= '9') {
                    unescaped = 4096 * (c - '0');
                } else if (c >= 'A' && c <= 'F') {
                    unescaped = 4096 * (10 + c - 'A');
                } else if (c >= 'a' && c <= 'f') {
                    unescaped = 4096 * (10 + c - 'a');
                } else {
                    throw new DeserializationException("Expected hex digit, found: " + c, reader);
                }

                c = reader.Read();
                if (c >= '0' && c <= '9') {
                    unescaped += 256 * (c - '0');
                } else if (c >= 'A' && c <= 'F') {
                    unescaped += 256 * (10 + c - 'A');
                } else if (c >= 'a' && c <= 'f') {
                    unescaped += 256 * (10 + c - 'a');
                } else {
                    throw new DeserializationException("Expected hex digit, found: " + c, reader);
                }

                c = reader.Read();
                if (c >= '0' && c <= '9') {
                    unescaped += 16 * (c - '0');
                } else if (c >= 'A' && c <= 'F') {
                    unescaped += 16 * (10 + c - 'A');
                } else if (c >= 'a' && c <= 'f') {
                    unescaped += 16 * (10 + c - 'a');
                } else {
                    throw new DeserializationException("Expected hex digit, found: " + c, reader);
                }

                c = reader.Read();
                if (c >= '0' && c <= '9') {
                    unescaped += c - '0';
                } else if (c >= 'A' && c <= 'F') {
                    unescaped += 10 + c - 'A';
                } else if (c >= 'a' && c <= 'f') {
                    unescaped += 10 + c - 'a';
                } else {
                    throw new DeserializationException("Expected hex digit, found: " + c, reader);
                }
                
                return (char)unescaped;
            }

            public static char ExpectUnicodeHexQuad(TextReader reader)
            {
                var c = reader.Read();
                if (c != 'u')
                    throw new DeserializationException("Escape sequence expected unicode hex quad", reader);

                return ReadHexQuad(reader);
            }
        }

        static MethodInfo TextReader_Read = typeof(TextReader).GetMethod("Read", Type.EmptyTypes);
        static MethodInfo Helper_Consume = typeof(Helper).GetMethod("Consume", new[] { typeof(TextReader), typeof(int), typeof(T) });
        static MethodInfo Helper_ExpectUnicodeHexQuad = typeof(Helper).GetMethod("ExpectUnicodeHexQuad", new[] { typeof(TextReader) });

        class Data
        {
            public readonly Action<Action> AddAction;
            public readonly Emit<Func<TextReader, T>> Emit;
            public readonly Action<Emit<Func<TextReader, T>>> DoReturn;
            public readonly Label Start;
            public readonly Label Failure;
            public readonly Local Local_ch;
            public readonly bool SkipTrailingWhitespace;
            public readonly bool FoldMultipleValues;

            public Data
                ( Action<Action> addAction
                , Emit<Func<TextReader, T>> emit
                , Action<Emit<Func<TextReader, T>>> doReturn
                , Label start
                , Label failure
                , Local local_ch
                , bool skipTrailingWhitespace
                , bool foldMultipleValues
                )
            {
                AddAction = addAction;
                Emit = emit;
                DoReturn = doReturn;
                Start = start;
                Failure = failure;
                Local_ch = local_ch;
                SkipTrailingWhitespace = skipTrailingWhitespace;
                FoldMultipleValues = foldMultipleValues;
            }
        }

        [DebuggerDisplay("{Name}")]
        public class AutomataName
        {
            public readonly string Name;
            public readonly Action<Emit<Func<TextReader, T>>> OnFound;

            public AutomataName(string name, Action<Emit<Func<TextReader, T>>> onFound)
            {
                Name = name;
                OnFound = onFound;
            }
        }

        static void FinishName(Data d, AutomataName nameValue, Label onMatchChar)
        {
            d.AddAction(() => 
            {
                d.Emit.MarkLabel(onMatchChar);
                nameValue.OnFound(d.Emit);
                d.DoReturn(d.Emit);
            });
        }

        private static void FoldName(Data d, AutomataName nameValue, Label foldName)
        {
            d.AddAction(() =>
            {
                d.Emit.MarkLabel(foldName);
                nameValue.OnFound(d.Emit);
                d.Emit.Branch(d.Start);
            });
        }


        static void NextChar(Data d, IList<AutomataName> nameValues, int pos, Label onMatchChar)
        {
            var chars =
                nameValues
                .GroupBy(nv => pos >= nv.Name.Length ? -1 : nv.Name[pos])
                .ToList();

            var namesToFinish = new List<Tuple<char, Label>>();
            foreach(var charGroup in chars)
            {
                var ch = charGroup.Key;
                var items = charGroup.ToList();
                if (ch == -1)
                {
                    if (charGroup.Count() != 1)
                    {
                        throw new ApplicationException("");
                    }

                    var completeName = d.Emit.DefineLabel("complete_" + items[0].Name);
                    FinishName(d, items[0], completeName);
                    namesToFinish.Add(Tuple.Create('"', completeName));

                    if (d.SkipTrailingWhitespace)
                        namesToFinish.Add(Tuple.Create(' ', onMatchChar));

                    if (d.FoldMultipleValues)
                    {
                        var foldName = d.Emit.DefineLabel("fold_" + items[0].Name);
                        FoldName(d, items[0], foldName);
                        namesToFinish.Add(Tuple.Create(',', foldName));
                    }
                }
                else
                {
                    var next = d.Emit.DefineLabel("_" + items[0].Name.Substring(0, pos + 1));
                    namesToFinish.Add(Tuple.Create((char)ch, next));
                    NextChar(d, items, pos + 1, next);
                }
            }

            d.AddAction(() =>
            {
                d.Emit.MarkLabel(onMatchChar);

                d.Emit.LoadArgument(0);
                d.Emit.CallVirtual(TextReader_Read);
                d.Emit.StoreLocal(d.Local_ch);

                Action checkChars = () =>
                {
                    // TODO: optimize this; use 'switch' and/or binary split depending on number/layout of characters
                    foreach (var item in namesToFinish)
                    {
                        d.Emit.LoadLocal(d.Local_ch);
                        d.Emit.LoadConstant((int)item.Item1);
                        d.Emit.BranchIfEqual(item.Item2);
                    }
                };

                checkChars();

                d.Emit.LoadLocal(d.Local_ch);               // char
                d.Emit.LoadConstant('\\');                  // char char
                d.Emit.UnsignedBranchIfNotEqual(d.Failure); // 
                d.Emit.LoadArgument(0);                     // TextReader
                d.Emit.Call(Helper_ExpectUnicodeHexQuad);   // char
                d.Emit.StoreLocal(d.Local_ch);              //

                checkChars();

                d.Emit.Branch(d.Failure);
            });
        }

        public static AutomataName CreateName(string name, Action<Emit<Func<TextReader, T>>> onFound)
        {
            return new AutomataName(name, onFound);
        }

        public static Func<TextReader, T> CreateFold(IEnumerable<AutomataName> names, Action<Emit<Func<TextReader, T>>> initialize, Action<Emit<Func<TextReader, T>>> doReturn, Action<Emit<Func<TextReader, T>>> errorValue, bool skipTrailingWhitespace, bool foldMultipleValues)
        {
            var sorted =
                names
                .OrderBy(kv => kv.Name)
                .ToList();

            var stack = new Stack<Action>();
            Action<Action> addAction =
                action => stack.Push(action);

            var emit = Emit<Func<TextReader, T>>.NewDynamicMethod(doVerify: Utils.DoVerify);

            initialize(emit);

            var ch = emit.DeclareLocal(typeof(int), "ch");
            var failure = emit.DefineLabel("failure");
            addAction(() =>
            {
                emit.MarkLabel(failure);
                emit.LoadArgument(0);
                emit.LoadLocal(ch);
                errorValue(emit);
                emit.Call(Helper_Consume);
                emit.Return();
            });

            var start = emit.DefineLabel("start");
            var d = new Data(addAction, emit, doReturn, start, failure, ch, skipTrailingWhitespace, foldMultipleValues);

            NextChar(d, sorted, 0, start);

            foreach (var action in stack)
            {
                action();
            }

            return emit.CreateDelegate(Utils.DelegateOptimizationOptions);
        }

        public static Func<TextReader, T> Create(IEnumerable<AutomataName> names, Action<Emit<Func<TextReader, T>>> errorValue)
        {
            return CreateFold(names, _ => { }, emit => emit.Return(), errorValue, false, false);
        }
    }
}
