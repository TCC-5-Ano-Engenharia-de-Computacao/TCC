using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Conditions
{
    [XmlTag("PlayerJustGroundedCondition")]
    public sealed class PlayerJustGroundedCondition : ConditionBase
    {
        private bool _prev;
        private Collider2D _coll;
        public override void Initialize(PlayerRoot owner) => _coll = owner.GetComponent<Collider2D>();
        public override bool Evaluate(PlayerRoot owner)
        {
            bool grounded = _coll && Physics2D.Raycast(owner.transform.position, Vector2.down, 0.1f);
            bool justGrounded = !_prev && grounded;
            _prev = grounded;
            return justGrounded;
        }
    }
}