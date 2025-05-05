using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Conditions
{
    namespace StateMachineExample.Gameplay.Conditions
    {
        /// <summary>
        /// Sempre retorna falso.
        /// </summary>
        [XmlTag("AlwaysFalseCondition")]
        public sealed class AlwaysFalseCondition : ConditionBase
        {
            public override void Initialize(RootCharacter owner) { }
            public override bool Evaluate(RootCharacter owner) => false;
        }
    }
}