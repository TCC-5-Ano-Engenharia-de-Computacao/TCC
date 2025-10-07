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
        public sealed class RecoverStaminaAction : ActionBase
        {
            [SerializeField] private float recoverRate = 10f;
            [SerializeField] private AttributeSystem attributeSystem;

            public override void Execute()
            {
                attributeSystem.RestoreStamina(recoverRate * Time.deltaTime);
            }

            public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
            {
                var go = new GameObject(nameof(RecoverStaminaAction));
                go.transform.SetParent(parent, false);

                var action = go.AddComponent<RecoverStaminaAction>();
                
                action.attributeSystem = player.attributeSystem;
                action.recoverRate =
                    float.TryParse((string)node.Attribute("recoverRate"),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var rr)
                        ? rr : action.recoverRate;
                
                return action;      
            }

            [RuntimeInitializeOnLoadMethod]
            private static void Register() =>
                ActionFactory.Register(nameof(RecoverStaminaAction), ConstructFromXmlAsync);
        }
    }

}