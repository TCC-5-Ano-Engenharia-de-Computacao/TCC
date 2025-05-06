using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Conditions
{
    [XmlTag("AnimationReachedEndCondition")]
    public sealed class AnimationReachedEndCondition : ConditionBase
    {
        private Animator _anim;
        [XmlAttribute("stateHash")] public int StateHash { get; set; }
        public override void Initialize(PlayerRoot owner) => _anim = owner.GetComponent<Animator>();
        public override bool Evaluate(PlayerRoot owner)
        {
            if (!_anim) return false;
            var info = _anim.GetCurrentAnimatorStateInfo(0);
            if (StateHash != 0 && info.shortNameHash != StateHash) return false;
            return info.normalizedTime >= 1f;
        }
    }
}