using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Conditions
{
    public sealed class AlwaysFalseCondition : ConditionBase
    {
        private PlayerRoot _player;

        public override bool Evaluate() => false;

        public static Task<ConditionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject("AlwaysFalseCondition");
            go.transform.SetParent(parent, false);

            var c = go.AddComponent<AlwaysFalseCondition>();
            c._player = player; // guarda o player se quiser usar no Evaluate()

            return Task.FromResult<ConditionBase>(c);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register("AlwaysFalseCondition", ConstructFromXmlAsync);
    }
}