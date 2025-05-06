using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Actions
{
    [XmlTag("JumpPlayerAction")]
    public sealed class JumpPlayerAction : ActionBase
    {
        [XmlAttribute("force")] public float JumpForce { get; set; } = 5f;
        public override void Execute(PlayerRoot owner)
        {
            var rb = owner.GetComponent<Rigidbody2D>();
            if (!rb) return;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    }
}