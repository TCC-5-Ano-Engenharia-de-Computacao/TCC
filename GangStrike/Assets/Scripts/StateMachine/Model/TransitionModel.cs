using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using StateMachine.Conditions;
using UnityEngine;

namespace StateMachine.Model
{
    public class TransitionModel
    {
        [XmlAttribute("to")] public string To { get; set; }

        [XmlArray("Conditions"), XmlArrayItem] public List<ConditionBase> Conditions { get; set; }

        public async Task Initialize(PlayerRoot playerRoot)
        {
            var tasks = new List<Task>();
            foreach (var condition in Conditions)
            {
                tasks.Add(condition.Initialize(playerRoot));
            }
            await Task.WhenAll(tasks);
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