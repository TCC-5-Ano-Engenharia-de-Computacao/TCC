namespace Player.NewStateMachine.Conditions
{
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;
    
    public class CanUseStaminaCondition : ConditionBase
    {
        private AttributeSystem attributeSystem;

        public override bool Evaluate()
        {
            return attributeSystem.canUseStamina;
        }

        // -------------------------- Fábrica / XML ---------------
        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(CanUseStaminaCondition));
            go.transform.SetParent(parent, false);

            var cond   = go.AddComponent<CanUseStaminaCondition>();
            cond.attributeSystem = player.attributeSystem;

            return Task.FromResult<ConditionBase>(cond);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(CanUseStaminaCondition), ConstructFromXmlAsync);
    }
}
