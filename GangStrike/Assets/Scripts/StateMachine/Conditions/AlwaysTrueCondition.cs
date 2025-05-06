using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Conditions
{
    /// <summary>
    /// Sempre retorna verdadeiro.
    /// </summary>
    [XmlTag("AlwaysTrueCondition")]
    public sealed class AlwaysTrueCondition : ConditionBase
    {
        public override void Initialize(PlayerRoot owner) { }
        public override bool Evaluate(PlayerRoot owner) => true;
    }
}