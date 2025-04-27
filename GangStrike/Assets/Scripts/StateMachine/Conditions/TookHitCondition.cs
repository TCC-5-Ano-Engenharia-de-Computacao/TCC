using System.Xml.Serialization;
using Examples;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Conditions
{
    [XmlTag("TookHitCondition")]
    public sealed class TookHitCondition : ConditionBase
    {
        [XmlAttribute("hitType")] public string HitType { get; set; } = "any";
        private PlayerHitbox _hitbox;
        public override void Initialize(GameObject owner) => _hitbox = owner.GetComponent<PlayerHitbox>();
        public override bool Evaluate(GameObject owner)
        {
            if (!_hitbox) return false;
            return _hitbox.TryConsumeHit(out string type) && (HitType == "any" || HitType == type);
        }
    }
}