namespace Player.NewStateMachine.Actions
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    namespace Player.NewStateMachine.Actions
    {
        public sealed class DrainStaminaAction : ActionBase
        {
            [SerializeField] private float drainRate = 20f;
            [SerializeField] private AttributeSystem attributeSystem;

            public override void Execute()
            {
                attributeSystem.ConsumeStamina(drainRate * Time.deltaTime);
            }

            public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
            {
                var go = new GameObject(nameof(DrainStaminaAction));
                go.transform.SetParent(parent, false);

                var action = go.AddComponent<DrainStaminaAction>();
                
                action.attributeSystem = player.attributeSystem;
                action.drainRate =
                    float.TryParse((string)node.Attribute("drainRate"),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var dr)
                        ? dr : action.drainRate;
                
                return action;      
            }

            [RuntimeInitializeOnLoadMethod]
            private static void Register() =>
                ActionFactory.Register(nameof(DrainStaminaAction), ConstructFromXmlAsync);
        }
    }

}