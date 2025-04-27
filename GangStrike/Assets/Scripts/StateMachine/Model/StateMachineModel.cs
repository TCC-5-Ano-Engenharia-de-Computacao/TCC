/*
================================================================================================
STATE‑MACHINE (Unity‑friendly)
-----------------------------------------------------------------------------------------------
• Todas as Actions terminam em "Action" e ficam no namespace StateMachineExample.Actions
• Todas as Conditions terminam em "Condition" e ficam no namespace StateMachineExample.Conditions
• Para estender: crie uma subclasse de ActionBase/ConditionBase, decore com [XmlTag("MinhaTag")]
  e coloque no Resources/StateMachines/<arquivo>.xml usando <MinhaTag .../>.
================================================================================================
*/



// ------------------------------------------------------------------------------------------------
//  MODELO (State, Transition, StateMachine)
// ------------------------------------------------------------------------------------------------
namespace StateMachine.Model
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("StateMachine")]
    public class StateMachineModel
    {
        [XmlAttribute("initialState")] public string InitialState { get; set; }

        [XmlArray("States"), XmlArrayItem("State")] public List<StateModel> States { get; set; }
    }
}
