using System.Collections.Generic;
using System.Xml.Serialization;
using StateMachine.Conditions;

namespace StateMachine.Model
{
    public class TransitionModel
    {
        [XmlAttribute("to")] public string To { get; set; }

        [XmlArray("Conditions"), XmlArrayItem] public List<ConditionBase> Conditions { get; set; }
    }
}