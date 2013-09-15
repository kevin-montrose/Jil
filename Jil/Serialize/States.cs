using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    abstract class State
    {
        private static long _Id;

        public long Id { get; private set; }

        public State NextState { get; private set; }
        public State PreviousState { get; private set; }

        protected State()
        {
            Id = Interlocked.Increment(ref _Id);
        }

        public void TransitionTo(State nextState)
        {
            if (NextState != null) throw new Exception("NextState already set");

            NextState = nextState;
            NextState.PreviousState = this;
        }

        public override abstract string ToString();
    }

    class TerminalState : State
    {
        public static TerminalState Instance { get { return new TerminalState(); } }

        private TerminalState() { }

        public override string ToString()
        {
            return "--terminate--";
        }
    }

    class InitialState : State
    {
        public static InitialState Instance { get { return new InitialState(); } }

        private InitialState() { }

        public override string ToString()
        {
            return "--initiate--";
        }
    }

    class StartObjectState : State
    {
        public static StartObjectState Instance { get { return new StartObjectState(); } }

        private StartObjectState() { }

        public override string ToString()
        {
            return "{";
        }
    }

    class EndObjectState : State
    {
        public static EndObjectState Instance { get { return new EndObjectState(); } }

        private EndObjectState() { }

        public override string ToString()
        {
            return "}";
        }
    }

    class StartListState : State
    {
        public static StartListState Instance { get { return new StartListState(); } }

        private StartListState() { }

        public override string ToString()
        {
            return "[";
        }
    }

    class EndListState : State
    {
        public static EndListState Instance { get { return new EndListState(); } }

        private EndListState() { }

        public override string ToString()
        {
            return "]";
        }
    }

    class CommaDeliminatorState : State
    {
        public static CommaDeliminatorState Instance { get { return new CommaDeliminatorState(); } }

        private CommaDeliminatorState() { }

        public override string ToString()
        {
            return ",";
        }
    }

    class WriteColonState : State
    {
        public static WriteColonState Instance { get { return new WriteColonState(); } }

        private WriteColonState() { }

        public override string ToString()
        {
            return ":";
        }
    }

    class WriteConstantStringState : State
    {
        public string String { get; private set; }
        public WriteConstantStringState(string str)
        {
            String = str;
        }

        public override string ToString()
        {
            return "\"" + String + "\"";
        }
    }

    abstract class MultiStateState : State
    {
        public IEnumerable<State> InnerStates { get; protected set; }

        public override string ToString()
        {
            return string.Join(" ", InnerStates);
        }
    }

    abstract class WriteNameValueState : MultiStateState
    {
        public Type Type { get; protected set; }
        public string Name { get; protected set; }
    }

    class WriteFieldState : WriteNameValueState
    {
        public FieldInfo Field { get; private set; }

        private WriteFieldState(FieldInfo field, IEnumerable<State> inner)
        {
            Type = field.FieldType;
            Name = field.Name;

            Field = field;

            InnerStates = inner;
        }

        public static WriteFieldState For(FieldInfo field)
        {
            var writeName = new WriteConstantStringState(field.Name);
            var writeColon = WriteColonState.Instance;
            var writeValueMachine = StateMachine.FromCache(field.FieldType);

            return new WriteFieldState(field, new State[] { writeName, writeColon, writeValueMachine });
        }
    }

    class WritePropertyState : WriteNameValueState
    {
        public PropertyInfo Property { get; private set; }
        private WritePropertyState(PropertyInfo prop, IEnumerable<State> inner)
        {
            Type = prop.PropertyType;
            Name = prop.Name;

            Property = prop;

            InnerStates = inner;
        }

        public static WritePropertyState For(PropertyInfo field)
        {
            var writeName = new WriteConstantStringState(field.Name);
            var writeColon = WriteColonState.Instance;
            var writeValueMachine = StateMachine.FromCache(field.PropertyType);

            return new WritePropertyState(field, new State[] { writeName, writeColon, writeValueMachine });
        }
    }


    class DelimitedListState : MultiStateState
    {
        public Type ValueType { get; private set; }
        private DelimitedListState(Type valueType, State writeValueState, State delimiter)
        {
            ValueType = valueType;
            InnerStates = new[] { writeValueState, delimiter };
        }

        public static DelimitedListState For(Type valType)
        {
            var writeValue = StateMachine.FromCache(valType);
            var delim = CommaDeliminatorState.Instance;

            return new DelimitedListState(valType, writeValue, delim);
        }
    }

    class DelimitedKeyValueListState : MultiStateState
    {
        public Type KeyType { get; private set; }
        public Type ValueType { get; private set; }
        private DelimitedKeyValueListState(Type keyType, Type valueType, State[] inner)
        {
            KeyType = keyType;
            ValueType = ValueType;

            InnerStates = inner;
        }

        public static DelimitedKeyValueListState For(Type keyType, Type valType)
        {
            State keyState = StateMachine.FromCache(keyType);
            State valState = StateMachine.FromCache(valType);

            return new DelimitedKeyValueListState(keyType, valType, new[] { keyState, WriteColonState.Instance, valState });
        }
    }

    class WriteByteState : State
    {
        public static WriteByteState Instance { get { return new WriteByteState(); } }

        private WriteByteState() { }

        public override string ToString()
        {
            return "+byte";
        }
    }

    class WriteSByteState : State
    {
        public static WriteSByteState Instance { get { return new WriteSByteState(); } }

        private WriteSByteState() { }

        public override string ToString()
        {
            return "+sbyte";
        }
    }

    class WriteShortState : State
    {
        public static WriteShortState Instance { get { return new WriteShortState(); } }

        private WriteShortState() { }

        public override string ToString()
        {
            return "+short";
        }
    }

    class WriteUShortState : State
    {
        public static WriteUShortState Instance { get { return new WriteUShortState(); } }

        private WriteUShortState() { }

        public override string ToString()
        {
            return "+ushort";
        }
    }

    class WriteIntState : State 
    {
        public static WriteIntState Instance { get { return new WriteIntState(); } }

        private WriteIntState() { }

        public override string ToString()
        {
            return "+int";
        }
    }

    class WriteUIntState : State
    {
        public static WriteUIntState Instance { get { return new WriteUIntState(); } }

        private WriteUIntState() { }

        public override string ToString()
        {
            return "+uint";
        }
    }

    class WriteLongState : State
    {
        public static WriteLongState Instance { get { return new WriteLongState(); } }

        private WriteLongState() { }

        public override string ToString()
        {
            return "+long";
        }
    }

    class WriteULongState : State
    {
        public static WriteULongState Instance { get { return new WriteULongState(); } }

        private WriteULongState() { }

        public override string ToString()
        {
            return "+ulong";
        }
    }

    class WriteFloatState : State
    {
        public static WriteFloatState Instance { get { return new WriteFloatState(); } }

        private WriteFloatState() { }

        public override string ToString()
        {
            return "+float";
        }
    }

    class WriteDoubleState : State
    {
        public static WriteDoubleState Instance { get { return new WriteDoubleState(); } }

        private WriteDoubleState() { }

        public override string ToString()
        {
            return "+double";
        }
    }

    class WriteDecimalState : State
    {
        public static WriteDecimalState Instance { get { return new WriteDecimalState(); } }

        private WriteDecimalState() { }

        public override string ToString()
        {
            return "+decimal";
        }
    }

    class WriteStringState : State
    {
        public static WriteStringState Instance { get { return new WriteStringState(); } }

        private WriteStringState() { }

        public override string ToString()
        {
            return "+string";
        }
    }
}
