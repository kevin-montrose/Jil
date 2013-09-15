using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    static class StateMachineCache<T>
    {
        public static StateMachine StateMachine;

        static StateMachineCache()
        {
            StateMachine = StateMachine.Empty();

            var actualMachine = StateMachine.BuildFor(typeof(T));

            StateMachine.ReplaceWith(actualMachine);
        }
    }
}
