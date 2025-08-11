using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Conditions
{
    public sealed class AlwaysTrueCondition : ConditionBase
    {
        public override bool Evaluate() => true;

        public static Task<ConditionBase> ConstructFromXmlAsync(XElement node, Transform parent,PlayerRoot playerRoot)
        {
            var go = new GameObject("AlwaysTrueCondition");
            go.transform.SetParent(parent, false);
            var c = go.AddComponent<AlwaysTrueCondition>();
            return Task.FromResult<ConditionBase>(c);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register("AlwaysTrueCondition", ConstructFromXmlAsync);
    }
}