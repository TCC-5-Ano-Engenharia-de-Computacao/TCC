// TransitionMono.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Player.NewStateMachine.Conditions;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Runtime
{
    public class TransitionMono : MonoBehaviour
    {
        [SerializeField] private string toId;
        [SerializeField] private List<ConditionBase> conditions = new();

        public string ToId => toId;

        public bool IsValid() => conditions.All(c => c.Evaluate());

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
            return t;
        }
    }
}