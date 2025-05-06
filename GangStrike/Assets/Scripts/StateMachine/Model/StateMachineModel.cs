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

using System.Linq;
using UnityEngine;

namespace StateMachine.Model
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("StateMachine")]
    public class StateMachineModel
    {
        [XmlAttribute("initialState")] public string InitialState { get; set; }

        [XmlArray("States"), XmlArrayItem("State")]
        public List<StateModel> States { get; set; }

        private Dictionary<string, StateModel> _stateLookup;

        public StateModel GetStateFromId(string id)
        {
            return _stateLookup.GetValueOrDefault(id);
        }

        public void Initialize(PlayerRoot playerRoot)
        {
            _stateLookup = States.ToDictionary(state => state.Id, state => state);

            foreach (var stateModel in States)
            {
                stateModel.Initialize(playerRoot);
            }
        }

        public string ToDebugString(int indentationLevel = 0)
        {
            var indentation = new string(' ', indentationLevel * 2);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(indentation + "StateMachineModel");
            sb.AppendLine(indentation + "initialState: " + InitialState);
            sb.AppendLine(indentation + "States:");
            foreach (var stateModel in States)
            {
                sb.AppendLine(stateModel.ToDebugString(indentationLevel + 1));
            }
            return sb.ToString();
        }

    }
}
