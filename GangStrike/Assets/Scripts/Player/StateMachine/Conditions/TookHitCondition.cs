using System.Threading.Tasks;
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
        public override async Task Initialize(PlayerRoot owner) => _hitbox = owner.GetComponent<PlayerHitbox>();
        public override bool Evaluate(PlayerRoot owner)
        {
            if (!_hitbox) return false;
            return _hitbox.TryConsumeHit(out string type) && (HitType == "any" || HitType == type);
        }

        public override string ToDebugString(int indentationLevel = 0)
        {
            var indentation = new string(' ', indentationLevel * 2);
            var baseDebugString = base.ToDebugString(indentationLevel);
            return $"{baseDebugString}\n{indentation}HitType: {HitType}";
        }
    }
}