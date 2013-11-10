using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    class CapacityEstimator
    {
        internal const int ListMultiplier = 10;
        internal const int DictionaryMultiplier = 10;
        internal const int StringEstimate = 20;
        internal const int CharacterEstimate = 1;
        internal const int IntEstimate = 3;
        internal const int BoolEstimate = 5;
        internal const int GuidEstimate = 36;
        internal const int DoubleEstimate = 3;

        public static int For(List<SerializingAction> actions)
        {
            var ret = 0;

            for (var i = 0; i < actions.Count; i++)
            {
                var act = actions[i];
                if (act is ListStartAction)
                {
                    var end = Find<ListStartAction, ListEndAction>(actions, i + 1);
                    var listActs = actions.Skip(i + 1).Take(end - i - 1).ToList();
                    // '{' + '}' + (',' x (ListMultiplied - 1))
                    var extra = 2 + (ListMultiplier - 1);
                    var singleElement = For(listActs);
                    ret += singleElement * ListMultiplier + extra;
                    i = end;
                    continue;
                }

                if (act is DictionaryStartAction)
                {
                    var end = Find<DictionaryStartAction, DictionaryEndAction>(actions, i + 1);
                    var dictActs = actions.Skip(i + 1).Take(end - i - 1).ToList();
                    // '{' + '}' + (',' x (DictionaryMultiplier - 1)
                    var extra = 2 + (DictionaryMultiplier - 1);
                    var singleElement = For(dictActs);
                    ret += singleElement * DictionaryMultiplier + extra;
                    i = end;
                    continue;
                }

                ret += For(act);
            }

            return ret;
        }

        static int For(SerializingAction act)
        {
            var asStr = act as WriteStringAction;
            if (asStr != null)
            {
                return asStr.String.Length;
            }

            if (act is WriteEncodedStringAction)
            {
                return StringEstimate;
            }

            if (act is WriteCharAction)
            {
                return CharacterEstimate;
            }

            if (act is WriteIntAction)
            {
                return IntEstimate;
            }

            if (act is WriteBoolAction)
            {
                return BoolEstimate;
            }

            if (act is WriteGuidAction)
            {
                return GuidEstimate;
            }

            if (act is WriteDoubleAction)
            {
                return DoubleEstimate;
            }

            throw new Exception("Unexpected SerializingAction: " + act);
        }

        static int Find<StartAct, EndAct>(List<SerializingAction> actions, int i)
            where StartAct : SerializingAction
            where EndAct : SerializingAction
        {
            var pending = 0;
            for (; i < actions.Count; i++)
            {
                var act = actions[i];
                if (act is StartAct)
                {
                    pending++;
                    continue;
                }

                if (act is EndAct)
                {
                    if (pending == 0) return i;
                    pending--;
                    continue;
                }
            }

            throw new Exception("No closing SerializingAction found, couldn't estimate capacity");
        }
    }
}
