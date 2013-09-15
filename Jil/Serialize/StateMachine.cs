using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    internal class StateMachine : State
    {
        public StateMachine(bool noInitialState = false)
        {
            if (noInitialState) return;

            TransitionTo(InitialState.Instance);
        }

        public void ReplaceWith(StateMachine other)
        {
            TransitionTo(other.NextState);
        }

        public static StateMachine Empty()
        {
            return new StateMachine(noInitialState: true);
        }

        private static StateMachine BuildDictionaryFor(Type dictType)
        {
            throw new NotImplementedException();
        }

        private static StateMachine BuildListFor(Type listType)
        {
            throw new NotImplementedException();
        }

        private static StateMachine For(State singleState)
        {
            var ret = new StateMachine();
            ret.TransitionTo(singleState);
            singleState.TransitionTo(TerminalState.Instance);

            return ret;
        }

        private static StateMachine BuildObjectFor(Type objType)
        {
            var ret = new StateMachine();
            
            var startObj = StartObjectState.Instance;
            ret.NextState.TransitionTo(startObj);

            var fields = objType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            var props = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.GetMethod != null);

            var writes = new List<WriteNameValueState>();

            foreach (var f in fields)
            {
                writes.Add(WriteFieldState.For(f));
            }

            foreach (var p in props)
            {
                writes.Add(WritePropertyState.For(p));
            }

            var propFieldUse = Utils.PropertyFieldUsage(objType);
            var fieldOrder = Utils.FieldOffsetsInMemory(objType);

            // Order property access based on how .NET decided to lay the object out in memory
            // Broadly speaking, we want to access things sequentially; although all other things being equal
            //   we'd rather access properties after fields under the theory that properties will benefit more
            //   from "priming" fields than the other way around.
            var orderedWrites = 
                writes.OrderBy(
                    w => 
                    {
                        var asField = w as WriteFieldState;
                        if (asField != null)
                        {
                            return fieldOrder[asField.Field];
                        }

                        var asProp = w as WritePropertyState;
                        if (asProp != null)
                        {
                            var fieldsUsed = propFieldUse[asProp.Property];
                            
                            // We couldn't figure out which fields are used, so take a chance
                            //   on faulting everything in
                            if(fieldsUsed.Count == 0) return int.MaxValue;

                            var minField = fieldsUsed.Select(f => fieldOrder[f]).Min();

                            return minField;
                        }

                        throw new Exception();
                    }
                ).ThenBy(
                    w =>
                    {
                        return w is WriteFieldState ? 0 : 1;
                    }
                ).ToList();

            State currentState = startObj;
            foreach (var write in orderedWrites)
            {
                if (currentState != startObj)
                {
                    var comma = CommaObjectState.Instance;
                    currentState.TransitionTo(comma);
                    currentState = comma;
                }

                currentState.TransitionTo(write);
                currentState = write;
            }

            var endObj = EndObjectState.Instance;
            currentState.TransitionTo(endObj);

            var terminalState = TerminalState.Instance;
            endObj.TransitionTo(terminalState);

            return ret;
        }

        public static StateMachine BuildFor(Type type)
        {
            if (type.IsDictionaryType())
            {
                return BuildDictionaryFor(type);
            }

            if (type.IsListType())
            {
                return BuildListFor(type);
            }

            if (type == typeof(string))
            {
                return For(WriteStringState.Instance);
            }

            if (type == typeof(byte))
            {
                return For(WriteByteState.Instance);
            }

            if (type == typeof(sbyte))
            {
                return For(WriteSByteState.Instance);
            }

            if (type == typeof(short))
            {
                return For(WriteShortState.Instance);
            }

            if (type == typeof(ushort))
            {
                return For(WriteUShortState.Instance);
            }

            if (type == typeof(int))
            {
                return For(WriteIntState.Instance);
            }

            if (type == typeof(uint))
            {
                return For(WriteUIntState.Instance);
            }

            if (type == typeof(long))
            {
                return For(WriteLongState.Instance);
            }

            if (type == typeof(ulong))
            {
                return For(WriteULongState.Instance);
            }

            if (type == typeof(float))
            {
                return For(WriteFloatState.Instance);
            }

            if (type == typeof(double))
            {
                return For(WriteDoubleState.Instance);
            }

            if (type == typeof(decimal))
            {
                return For(WriteDecimalState.Instance);
            }

            return BuildObjectFor(type);   
        }
    }
}
