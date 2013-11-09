using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jil.Serialize
{
    class CapacityEstimator
    {
        const int ListMultiplier = 3;
        const int DictionaryMultiplier = 3;
        const int StringEstimate = 20;
        const int CharacterEstimate = 1;
        const int IntEstimate = 3;
        const int BoolEstimate = 5;
        const int GuidEstimate = 36;

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
                    ret += For(listActs) * ListMultiplier;
                    i = end;
                    continue;
                }

                if (act is DictionaryStartAction)
                {
                    var end = Find<DictionaryStartAction, DictionaryEndAction>(actions, i + 1);
                    var dictActs = actions.Skip(i + 1).Take(end - i - 1).ToList();
                    ret += For(dictActs) * DictionaryMultiplier;
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
