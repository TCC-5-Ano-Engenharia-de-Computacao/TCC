using System;

namespace StateMachine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class XmlTagAttribute : Attribute
    {
        public string ElementName { get; }
        public XmlTagAttribute(string elementName) => ElementName = elementName;
    }
}