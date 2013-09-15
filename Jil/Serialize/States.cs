using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    abstract class State
    {
        public State NextState { get; private set; }

        public void TransitionTo(State nextState)
        {
            if (NextState != null) throw new Exception("NextState already set");

            NextState = nextState;
        }
    }

    class TerminalState : State
    {
        public static TerminalState Instance { get { return new TerminalState(); } }

        private TerminalState() { }
    }

    class InitialState : State
    {
        public static InitialState Instance { get { return new InitialState(); } }

        private InitialState() { }
    }

    class StartObjectState : State
    {
        public static StartObjectState Instance { get { return new StartObjectState(); } }

        private StartObjectState() { }
    }

    class EndObjectState : State
    {
        public static EndObjectState Instance { get { return new EndObjectState(); } }

        private EndObjectState() { }
    }

    class CommaObjectState : State
    {
        public static CommaObjectState Instance { get { return new CommaObjectState(); } }

        private CommaObjectState() { }
    }

    class WriteColonState : State
    {
        public static WriteColonState Instance { get { return new WriteColonState(); } }

        private WriteColonState() { }
    }

    class WriteConstantStringState : State
    {
        public string String { get; private set; }
        public WriteConstantStringState(string str)
        {
            String = str;
        }
    }

    abstract class WriteNameValueState : State
    {
        public Type Type { get; protected set; }
        public string Name { get; protected set; }

        public IEnumerable<State> InnerStates { get; protected set; }
    }

    class WriteFieldState : WriteNameValueState
    {
        public FieldInfo Field { get; private set; }

        private WriteFieldState(FieldInfo field, IEnumerable<State> inner)
        {
            Type = field.FieldType;
            Name = field.Name;

            Field = field;
        }

        public static WriteFieldState For(FieldInfo field)
        {
            var writeName = new WriteConstantStringState(field.Name);
            var writeColon = WriteColonState.Instance;
            var writeValueMachine = StateMachine.BuildFor(field.FieldType);

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
        }

        public static WritePropertyState For(PropertyInfo field)
        {
            var writeName = new WriteConstantStringState(field.Name);
            var writeColon = WriteColonState.Instance;
            var writeValueMachine = StateMachine.BuildFor(field.PropertyType);

            return new WritePropertyState(field, new State[] { writeName, writeColon, writeValueMachine });
        }
    }

    class WriteByteState : State
    {
        public static WriteByteState Instance { get { return new WriteByteState(); } }

        private WriteByteState() { }
    }

    class WriteSByteState : State
    {
        public static WriteSByteState Instance { get { return new WriteSByteState(); } }

        private WriteSByteState() { }
    }

    class WriteShortState : State
    {
        public static WriteShortState Instance { get { return new WriteShortState(); } }

        private WriteShortState() { }
    }

    class WriteUShortState : State
    {
        public static WriteUShortState Instance { get { return new WriteUShortState(); } }

        private WriteUShortState() { }
    }

    class WriteIntState : State 
    {
        public static WriteIntState Instance { get { return new WriteIntState(); } }

        private WriteIntState() { }
    }

    class WriteUIntState : State
    {
        public static WriteUIntState Instance { get { return new WriteUIntState(); } }

        private WriteUIntState() { }
    }

    class WriteLongState : State
    {
        public static WriteLongState Instance { get { return new WriteLongState(); } }

        private WriteLongState() { }
    }

    class WriteULongState : State
    {
        public static WriteULongState Instance { get { return new WriteULongState(); } }

        private WriteULongState() { }
    }

    class WriteFloatState : State
    {
        public static WriteFloatState Instance { get { return new WriteFloatState(); } }

        private WriteFloatState() { }
    }

    class WriteDoubleState : State
    {
        public static WriteDoubleState Instance { get { return new WriteDoubleState(); } }

        private WriteDoubleState() { }
    }

    class WriteDecimalState : State
    {
        public static WriteDecimalState Instance { get { return new WriteDecimalState(); } }

        private WriteDecimalState() { }
    }

    class WriteStringState : State
    {
        public static WriteStringState Instance { get { return new WriteStringState(); } }

        private WriteStringState() { }
    }
}
