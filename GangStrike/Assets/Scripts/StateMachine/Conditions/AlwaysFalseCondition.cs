using System.Threading.Tasks;
using StateMachine.Attributes;

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
            public override async Task Initialize(PlayerRoot owner) { }
            public override bool Evaluate(PlayerRoot owner) => false;
        }
    }
}