namespace Player.NewStateMachine.Conditions
{
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;
    
    public class CheckStaminaExhaustedCondition : ConditionBase
    {
        private AttributeSystem attributeSystem;

        public override bool Evaluate()
        {
            if (attributeSystem.staminaExhausted)
            {
                attributeSystem.staminaExhausted = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        // -------------------------- Fábrica / XML ---------------
        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(CheckStaminaExhaustedCondition));
            go.transform.SetParent(parent, false);

            var cond   = go.AddComponent<CheckStaminaExhaustedCondition>();
            cond.attributeSystem = player.attributeSystem;

            return Task.FromResult<ConditionBase>(cond);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(CheckStaminaExhaustedCondition), ConstructFromXmlAsync);
    }
}
