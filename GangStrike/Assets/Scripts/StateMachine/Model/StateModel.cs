using System.Collections.Generic;
using System.Xml.Serialization;
using StateMachine.Actions;

namespace StateMachine.Model
{
    public class StateModel
    {
        [XmlAttribute("id")] public string Id { get; set; }

        [XmlArray("BeforeEnter"), XmlArrayItem] public List<ActionBase> BeforeEnter { get; set; }
        [XmlArray("OnEnter"),     XmlArrayItem] public List<ActionBase> OnEnter     { get; set; }
        [XmlArray("OnStay"),      XmlArrayItem] public List<ActionBase> OnStay      { get; set; }
        [XmlArray("OnLeave"),     XmlArrayItem] public List<ActionBase> OnLeave     { get; set; }

        [XmlArray("Transitions"), XmlArrayItem("Transition")] public List<TransitionModel> Transitions { get; set; }
    }
}