/*
using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;


namespace StateMachine.Actions
{
    [XmlTag("ConsumeInputAction")]
    public class ConsumeInputAction:ActionBase
    {
        [XmlAttribute("inputName")] public string inputName { get; set; } = "Jump";
        public override void Execute(PlayerRoot owner)
        {
            owner.inputRoot.inputBuffer.ConsumeInput(inputName);
        }
        
        public override string ToDebugString(int indentationLevel = 0)
        {
            var indentation = new string(' ', indentationLevel * 2);
            return indentation + GetType().Name;
        }
    }
}*/