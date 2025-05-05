using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Conditions
{
    [XmlTag("PlayerIsFallingCondition")]
    public sealed class PlayerIsFallingCondition : ConditionBase
    {
        private Rigidbody2D _rb;
        public override void Initialize(RootCharacter owner) => _rb = owner.GetComponent<Rigidbody2D>();
        public override bool Evaluate(RootCharacter owner) => _rb && _rb.linearVelocity.y < -0.01f;
    }
}