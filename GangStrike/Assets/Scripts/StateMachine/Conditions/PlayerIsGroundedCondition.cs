using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Conditions
{
    [XmlTag("PlayerIsGroundedCondition")]
    public sealed class PlayerIsGroundedCondition : ConditionBase
    {
        private Collider2D _coll;
        public override void Initialize(GameObject owner) => _coll = owner.GetComponent<Collider2D>();
        public override bool Evaluate(GameObject owner) => _coll && Physics2D.Raycast(owner.transform.position, Vector2.down, 0.1f);
    }
}