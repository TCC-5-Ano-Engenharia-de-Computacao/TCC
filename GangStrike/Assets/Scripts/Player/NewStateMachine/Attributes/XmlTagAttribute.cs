// ==============================================
// Anotação de tag para mapear XML -> Tipo
// ==============================================

using System;
using StateMachine;
using ActionBase = Player.NewStateMachine.Actions.ActionBase;
using ConditionBase = Player.NewStateMachine.Conditions.ConditionBase;
using RuleTile = UnityEngine.RuleTile;
using Task = UnityEditor.VersionControl.Task;

namespace Player.NewStateMachine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class XmlTagAttribute : Attribute
    {
        public string ElementName { get; }
        public XmlTagAttribute(string elementName) => ElementName = elementName;
    }
}
