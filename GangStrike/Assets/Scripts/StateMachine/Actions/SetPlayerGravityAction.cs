using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Actions
{
    [XmlTag("SetPlayerGravityAction")]
    public sealed class SetPlayerGravityAction : ActionBase
    {
        [XmlAttribute("scale")] public float GravityScale { get; set; } = 1f;
        public override void Execute(GameObject owner)
        {
            var rb = owner.GetComponent<Rigidbody2D>();
            if (rb) rb.gravityScale = GravityScale;
        }
    }
}