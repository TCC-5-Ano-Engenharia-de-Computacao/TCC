using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Actions
{
    [XmlTag("ClearInputBufferAction")]
    public sealed class ClearInputBufferAction:ActionBase
    {
        public override void Execute(PlayerRoot owner)
        {
            owner.inputRoot.inputBuffer.ClearInputBuffer();
        }
        
        public override string ToDebugString(int indentationLevel = 0)
        {
            var indentation = new string(' ', indentationLevel * 2);
            return indentation + GetType().Name;
        }
    }
}