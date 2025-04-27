using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Actions
{
    [XmlTag("LogAction")]
    public sealed class LogAction : ActionBase
    {
        [XmlAttribute("msg")] public string Message { get; set; }
        public override void Execute(GameObject owner) => Debug.Log(Message);
    }
}