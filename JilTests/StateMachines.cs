using Jil.Serialize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JilTests
{
    [TestClass]
    public class StateMachines
    {
        [TestMethod]
        public void Simple()
        {
            {
                var sm = StateMachine.FromCache(typeof(byte));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteByteState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(sbyte));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteSByteState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(short));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteShortState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(ushort));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteUShortState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(int));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteIntState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(uint));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteUIntState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(long));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteLongState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(ulong));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteULongState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(float));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteFloatState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(double));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteDoubleState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(decimal));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteDecimalState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }

            {
                var sm = StateMachine.FromCache(typeof(string));

                Assert.IsTrue(sm.NextState is InitialState);
                Assert.IsTrue(sm.NextState.NextState is WriteStringState);
                Assert.IsTrue(sm.NextState.NextState.NextState is TerminalState);
            }
        }

        class _Cyclical
        {
            public string Boring { get; set; }
            public _Cyclical Next { get; set; }
        }

        [TestMethod]
        public void Cyclical()
        {
            var machine = StateMachine.FromCache(typeof(_Cyclical));

            Assert.IsNotNull(machine);
        }
    }
}
