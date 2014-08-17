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

        static void Recurse(Action<Action> addAction, Emit<Func<TextReader, T>> emit, Label failure, Local local_ch, Label marking, IList<Tuple<string, Action<Emit<Func<TextReader, T>>>>> nameValues, int pos)
        {
            var i = 0;

            var chars =
                nameValues
                .Select(nv => pos >= nv.Item1.Length ? -1 : nv.Item1[pos])
                .Distinct()
                .ToList();

            if (chars.Count == 1)
            {
                var ch = chars[0];
                if (ch == -1)
                {
                    addAction(() =>
                    {
                        if (marking != null)
                            emit.MarkLabel(marking);

                        nameValues[0].Item2(emit);
                        emit.Return();
                    });
                }
                else
                {
                    var next = emit.DefineLabel("_" + nameValues[0].Item1.Substring(0, pos + 1));
                    Recurse(addAction, emit, failure, local_ch, next, nameValues, pos + 1);

                    addAction(() =>
                    {
                        if (marking != null)
                            emit.MarkLabel(marking);
                        emit.LoadArgument(0);                   // TextReader
                        emit.CallVirtual(TextReader_Read);      // char
                        emit.StoreLocal(local_ch);              //

                        Action checkChar = () =>
                        {
                            emit.LoadLocal(local_ch);               // char
                            emit.LoadConstant(ch);                  // char char
                            emit.BranchIfEqual(next);               //
                        };

                        checkChar();

                        emit.LoadLocal(local_ch);               // char
                        emit.LoadConstant('\\');                // char char
                        emit.UnsignedBranchIfNotEqual(failure); // 
                        emit.LoadArgument(0);                   // TextReader
                        emit.Call(Helper_ExpectUnicodeHexQuad); // char
                        emit.StoreLocal(local_ch);              //

                        checkChar();

                        emit.Branch(failure);
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
                    var next = emit.DefineLabel("_" + nameValues[i].Item1.Substring(0, pos + 1));
                    var j = i + 1;
                    var subset = (new[] { nameValues[i] }).ToList();
                    var ch = nameValues[i].Item1[pos];
                    while (j < nameValues.Count)
                    {
                        if (ch != nameValues[j].Item1[pos])
                            break;
                        subset.Add(nameValues[j]);
                        ++j;
                    }
                    namesToFinish.Add(Tuple.Create(ch, next, subset));
                    Recurse(addAction, emit, failure, local_ch, next, subset, pos + 1);
                    i = j;
                }

                addAction(() => emit.Branch(failure));

                Action checkChars = () =>
                {
                    // TODO: optimize this; use 'switch' and/or binary split depending on number/layout of characters
                    foreach (var item in namesToFinish)
                    {
                        addAction(() =>
                        {
                            emit.LoadLocal(local_ch);
                            emit.LoadConstant((int)item.Item1);
                            emit.BranchIfEqual(item.Item2);
                        });
                    }
                };
                
                checkChars();

                addAction(() =>
                {
                    emit.LoadLocal(local_ch);               // char
                    emit.LoadConstant('\\');                // char char
                    emit.UnsignedBranchIfNotEqual(failure); // 
                    emit.LoadArgument(0);                   // TextReader
                    emit.Call(Helper_ExpectUnicodeHexQuad); // char
                    emit.StoreLocal(local_ch);              //
                });

                checkChars();

                addAction(() =>
                {
                    if (marking != null)
                    {
                        emit.MarkLabel(marking);
                        marking = null;
                    }
                    emit.LoadArgument(0);
                    emit.CallVirtual(TextReader_Read);
                    emit.StoreLocal(local_ch);
                });
            }
        }

        public static Func<TextReader, T> Create(IList<Tuple<string, Action<Emit<Func<TextReader, T>>>>> names, Action<Emit<Func<TextReader, T>>> errorValue)
        {
            var sorted =
                names
                .Select(kv => Tuple.Create(kv.Item1 + '\"', kv.Item2))
                .OrderBy(kv => kv.Item1)
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

            Recurse(addAction, emit, failure, ch, null, sorted, 0);

            foreach (var action in stack)
            {
                action();
            }

            return emit.CreateDelegate(Utils.DelegateOptimizationOptions);
        }
    }
}
