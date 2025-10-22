// ==============================================
// Anotação de tag para mapear XML -> Tipo
// ==============================================

using System;

namespace Player.NewStateMachine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class XmlTagAttribute : Attribute
    {
        public string ElementName { get; }
        public XmlTagAttribute(string elementName) => ElementName = elementName;
    }
}
