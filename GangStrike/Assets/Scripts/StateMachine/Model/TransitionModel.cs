using System.Collections.Generic;
using System.Xml.Serialization;
using StateMachine.Conditions;
using UnityEngine;

namespace StateMachine.Model
{
    public class TransitionModel
    {
        [XmlAttribute("to")] public string To { get; set; }

        [XmlArray("Conditions"), XmlArrayItem] public List<ConditionBase> Conditions { get; set; }

        public void Initialize(RootCharacter rootCharacter)
        {
            foreach (var condition in Conditions)
            {
                condition.Initialize(rootCharacter);
            }
        }


        public string ToDebugString(int indentationLevel = 0)
        {
            var indentation = new string(' ', indentationLevel * 2);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(indentation + "TransitionModel");
            sb.AppendLine(indentation + "to: " + To);
            sb.AppendLine(indentation + "Conditions:");
            foreach (var condition in Conditions)
            {
                sb.AppendLine(condition.ToDebugString(indentationLevel + 1));
            }
            return sb.ToString();
            
        }

      
    }
}