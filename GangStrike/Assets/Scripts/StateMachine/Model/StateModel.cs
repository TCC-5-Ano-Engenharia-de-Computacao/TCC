using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using StateMachine.Actions;
using UnityEngine;

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

        public async Task Initialize(PlayerRoot playerRoot)
        {
            var tasks = new List<Task>();

            foreach (var action in BeforeEnter)
                tasks.Add(action.Initialize(playerRoot));
            foreach (var action in OnEnter)
                tasks.Add(action.Initialize(playerRoot));
            foreach (var action in OnStay)
                tasks.Add(action.Initialize(playerRoot));
            foreach (var action in OnLeave)
                tasks.Add(action.Initialize(playerRoot));
            foreach (var transition in Transitions)
                tasks.Add(transition.Initialize(playerRoot));

            await Task.WhenAll(tasks);
        }

        public TransitionModel EvaluateTransitions(PlayerRoot playerRoot)
        {
           
            foreach (var transitionModel in Transitions)
            {
                var flag = true;
                foreach (var condition in transitionModel.Conditions)
                {
                    if (!condition.Evaluate(playerRoot))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    return transitionModel;
                }
            }
            return null;
        }   
     
        public void DoBeforeEnter(PlayerRoot playerRoot)
        {
            foreach (var action in BeforeEnter)
            {
                action.Execute(playerRoot);
            }
        }

        public void DoEnter(PlayerRoot playerRoot)
        {
            foreach (var action in OnEnter)
            {
                action.Execute(playerRoot);
            }
        }
        
        public void DoLeave(PlayerRoot playerRoot)
        {
            foreach (var action in OnLeave)
            {
                action.Execute(playerRoot);
            }
        }

        public void DoStay(PlayerRoot playerRoot)
        {
            foreach (var action in OnStay)
            {
                action.Execute(playerRoot);
            }
        }

        public string ToDebugString(int indentationLevel = 0)
        {
            var indentation = new string(' ', indentationLevel * 2);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(indentation + "StateModel");
            sb.AppendLine(indentation + "id: " + Id);
            sb.AppendLine(indentation + "BeforeEnter: ");
            foreach (var action in BeforeEnter)
            {
                sb.AppendLine(action.ToDebugString(indentationLevel + 1));
            }
            sb.AppendLine(indentation + "OnEnter: ");
            foreach (var action in OnEnter)
            {
                sb.AppendLine(action.ToDebugString(indentationLevel + 1));
            }
            sb.AppendLine(indentation + "OnStay: ");
            foreach (var action in OnStay)
            {
                sb.AppendLine(action.ToDebugString(indentationLevel + 1));
            }
            sb.AppendLine(indentation + "OnLeave: ");
            foreach (var action in OnLeave)
            {
                sb.AppendLine(action.ToDebugString(indentationLevel + 1));
            }
            sb.AppendLine(indentation + "Transitions: ");
            foreach (var transition in Transitions)
            {
                sb.AppendLine(transition.ToDebugString(indentationLevel + 1));
            }
            return sb.ToString();
            
        }
        
    }
}