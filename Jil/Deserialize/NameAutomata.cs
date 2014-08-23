using Jil.Common;
using Sigil;
using Sigil.NonGeneric;
using System;
using System.Collections.Generic;
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
            public readonly Label Failure;
            public readonly Local Local_ch;

            public Data(Action<Action> addAction, Emit<Func<TextReader, T>> emit, Label failure, Local local_ch)
            {
                AddAction = addAction;
                Emit = emit;
                Failure = failure;
                Local_ch = local_ch;
            }
        }

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

        static void Recurse(Data d, IList<AutomataName> nameValues, int pos, Label onMatchChar)
        {
            var i = 0;

            var chars =
                nameValues
                .Select(nv => pos >= nv.Name.Length ? -1 : nv.Name[pos])
                .Distinct()
                .ToList();

            if (chars.Count == 1)
            {
                var ch = chars[0];
                if (ch == -1)
                {
                    d.AddAction(() =>
                    {
                        d.Emit.MarkLabel(onMatchChar);
                        nameValues[0].OnFound(d.Emit);
                        d.Emit.Return();
                    });
                }
                else
                {
                    var next = d.Emit.DefineLabel("_" + nameValues[0].Name.Substring(0, pos + 1));
                    Recurse(d, nameValues, pos + 1, next);

                    d.AddAction(() =>
                    {
                        if (onMatchChar != null)
                            d.Emit.MarkLabel(onMatchChar);
                        d.Emit.LoadArgument(0);              // TextReader
                        d.Emit.CallVirtual(TextReader_Read); // char
                        d.Emit.StoreLocal(d.Local_ch);       //

                        Action checkChar = () =>
                        {
                            d.Emit.LoadLocal(d.Local_ch);    // char
                            d.Emit.LoadConstant(ch);         // char char
                            d.Emit.BranchIfEqual(next);      //
                        };

                        checkChar();

                        d.Emit.LoadLocal(d.Local_ch);               // char
                        d.Emit.LoadConstant('\\');                  // char char
                        d.Emit.UnsignedBranchIfNotEqual(d.Failure); // 
                        d.Emit.LoadArgument(0);                     // TextReader
                        d.Emit.Call(Helper_ExpectUnicodeHexQuad);   // char
                        d.Emit.StoreLocal(d.Local_ch);              //

                        checkChar();

                        d.Emit.Branch(d.Failure);
                    });
                }
            }
            else if (chars.Count > 1)
            {
                var namesToFinish =
                    Enumerable
                    .Repeat(Tuple.Create(default(char), default(Label), new[] { nameValues[0] }.ToList()), 0)
                    .ToList();
                while (i < nameValues.Count)
                {
                    var next = d.Emit.DefineLabel("_" + nameValues[i].Name.Substring(0, pos + 1));
                    var j = i + 1;
                    var subset = (new[] { nameValues[i] }).ToList();
                    var ch = nameValues[i].Name[pos];
                    while (j < nameValues.Count)
                    {
                        if (ch != nameValues[j].Name[pos])
                            break;
                        subset.Add(nameValues[j]);
                        ++j;
                    }
                    namesToFinish.Add(Tuple.Create(ch, next, subset));
                    Recurse(d, subset, pos + 1, next);
                    i = j;
                }

                d.AddAction(() => d.Emit.Branch(d.Failure));

                Action checkChars = () =>
                {
                    // TODO: optimize this; use 'switch' and/or binary split depending on number/layout of characters
                    foreach (var item in namesToFinish)
                    {
                        d.AddAction(() =>
                        {
                            d.Emit.LoadLocal(d.Local_ch);
                            d.Emit.LoadConstant((int)item.Item1);
                            d.Emit.BranchIfEqual(item.Item2);
                        });
                    }
                };
                
                checkChars();

                d.AddAction(() =>
                {
                    d.Emit.LoadLocal(d.Local_ch);               // char
                    d.Emit.LoadConstant('\\');                  // char char
                    d.Emit.UnsignedBranchIfNotEqual(d.Failure); // 
                    d.Emit.LoadArgument(0);                     // TextReader
                    d.Emit.Call(Helper_ExpectUnicodeHexQuad);   // char
                    d.Emit.StoreLocal(d.Local_ch);              //
                });

                checkChars();

                d.AddAction(() =>
                {
                    if (onMatchChar != null)
                    {
                        d.Emit.MarkLabel(onMatchChar);
                        onMatchChar = null;
                    }
                    d.Emit.LoadArgument(0);
                    d.Emit.CallVirtual(TextReader_Read);
                    d.Emit.StoreLocal(d.Local_ch);
                });
            }
        }

        public static AutomataName CreateName(string name, Action<Emit<Func<TextReader, T>>> onFound)
        {
            return new AutomataName(name, onFound);
        }

        public static Func<TextReader, T> Create(IList<AutomataName> names, Action<Emit<Func<TextReader, T>>> errorValue)
        {
            var sorted =
                names
                .Select(kv => new AutomataName(kv.Name + '\"', kv.OnFound))
                .OrderBy(kv => kv.Name)
                .ToList();

            var stack = new Stack<Action>();
            Action<Action> addAction =
                action => stack.Push(action);

            var emit = Emit<Func<TextReader, T>>.NewDynamicMethod(doVerify: Utils.DoVerify);

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

            var d = new Data(addAction, emit, failure, ch);

            Recurse(d, sorted, 0, null);

            foreach (var action in stack)
            {
                action();
            }

            return emit.CreateDelegate(Utils.DelegateOptimizationOptions);
        }
    }
}
