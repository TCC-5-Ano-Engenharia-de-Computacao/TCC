using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Conditions
{
    [XmlTag("PlayerJustFallingCondition")]
    public sealed class PlayerJustFallingCondition : ConditionBase
    {
        private bool _prev;
        private Rigidbody2D _rb;
        public override void Initialize(GameObject owner) => _rb = owner.GetComponent<Rigidbody2D>();
        public override bool Evaluate(GameObject owner)
        {
            bool falling = _rb && _rb.linearVelocity.y < -0.01f;
            bool justFalling = !_prev && falling;
            _prev = falling;
            return justFalling;
        }
    }
}