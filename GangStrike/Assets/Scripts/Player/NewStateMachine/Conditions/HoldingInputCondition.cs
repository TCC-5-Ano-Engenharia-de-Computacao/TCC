namespace Player.NewStateMachine.Conditions
{
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;
    using Input; 
    
    public sealed class HoldingInputCondition : ConditionBase
    {
        // ------------------------------- Inspector
        [SerializeField] private string actionName = "Block";
        [SerializeField] private bool holding = true;

        // ------------------------------- Runtime
        private NewInputBuffer _buffer;

        public override bool Evaluate()
        {
            return holding == _buffer.IsHolding(actionName);
        }

        // -------------------------- Fábrica / XML ---------------
        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(HoldingInputCondition));
            go.transform.SetParent(parent, false);

            var cond   = go.AddComponent<HoldingInputCondition>();
            cond._buffer = player.inputRoot.inputBuffer;

            string? attr;

            attr = (string?)node.Attribute("inputName");
            if (!string.IsNullOrEmpty(attr)) cond.actionName = attr;
            
            attr = (string?)node.Attribute("holding");
            if (!string.IsNullOrEmpty(attr) && bool.TryParse(attr, out var hold))
                cond.holding = hold;

            return Task.FromResult<ConditionBase>(cond);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(HoldingInputCondition), ConstructFromXmlAsync);
    }
}
