namespace Player.NewStateMachine.Actions
{
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    namespace Player.NewStateMachine.Actions
    {
        public sealed class SpendUltimateAction : ActionBase
        {
            [SerializeField] private float amount = 60f;
            [SerializeField] private AttributeSystem attributeSystem;

            public override void Execute()
            {
                attributeSystem.ConsumeUltimate(amount);
            }

            public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
            {
                var go = new GameObject(nameof(SpendUltimateAction));
                go.transform.SetParent(parent, false);

                var action = go.AddComponent<SpendUltimateAction>();
                
                action.attributeSystem = player.attributeSystem;
                action.amount =
                    float.TryParse((string)node.Attribute("amount"),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var am)
                        ? am : action.amount;
                
                return action;      
            }

            [RuntimeInitializeOnLoadMethod]
            private static void Register() =>
                ActionFactory.Register(nameof(SpendUltimateAction), ConstructFromXmlAsync);
        }
    }

}