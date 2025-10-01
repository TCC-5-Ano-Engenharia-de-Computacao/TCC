// TransitionMono.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Player.NewStateMachine.Actions;
using Player.NewStateMachine.Conditions;
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.NewStateMachine.Runtime
{
    public class TransitionMono : MonoBehaviour
    {
        [SerializeField] private string toId;
        [SerializeField] private List<ConditionBase> conditions = new();
        [SerializeField] private List<ActionBase> successActions = new();
        [SerializeField] private List<ActionBase> failActions = new();
        
        public string ToId => toId;

        public bool IsValid() => conditions.All(c => c.Evaluate());

        public void OnSuccess()
        {
            foreach (var a in successActions) a.Execute();
        }
        
        public void OnFail()
        {
            foreach (var a in failActions) a.Execute();
        }
        
        public static async Task<TransitionMono> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject($"Transition_to_{(string)node.Attribute("to")}");
            go.transform.SetParent(parent, false);
            var t = go.AddComponent<TransitionMono>();
            t.toId = (string)node.Attribute("to");

            var condsNode = node.Element("Conditions");
            
            if (condsNode != null)
            {
                var created = await Task.WhenAll(
                    condsNode.Elements().Select(n => ConditionFactory.CreateAsync(n, go.transform, player))
                );
                foreach (var c in created) if (c != null) t.conditions.Add(c);
            }
            
            var successActions = node.Element("SuccessActions");
            
            if (successActions != null)
            {
                var created = await Task.WhenAll(
                    successActions.Elements().Select(n => ActionFactory.CreateAsync(n, go.transform, player))
                );
                foreach (var a in created) if (a != null) t.successActions.Add(a);
            }
            
            var failActions = node.Element("FailActions");
            
            if (failActions != null)
            {
                var created = await Task.WhenAll(
                    failActions.Elements().Select(n => ActionFactory.CreateAsync(n, go.transform, player))
                );
                foreach (var a in created) if (a != null) t.failActions.Add(a);
            }
            
            return t;
        }
    }
}