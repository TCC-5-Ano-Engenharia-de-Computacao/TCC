using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Actions
{
    [XmlTag("MovePlayerAction")]
    public sealed class MovePlayerAction : ActionBase
    {
        [XmlAttribute("speed")]     public float Speed { get; set; } = 5f;
        [XmlAttribute("direction")] public string Direction { get; set; } = "right"; // left|right
        public override void Execute(RootCharacter owner)
        {
            var rb = owner.GetComponent<Rigidbody2D>();
            if (!rb) return;
            var dir = Direction.ToLower().StartsWith("l") ? -1f : 1f;
            rb.linearVelocity = new Vector2(dir * Speed, rb.linearVelocity.y);
        }
    }
}