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
        public override void Initialize(RootCharacter owner) { }
        public override bool Evaluate(RootCharacter owner) => true;
    }
}