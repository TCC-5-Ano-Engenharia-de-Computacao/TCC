// StateMono.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Player.NewStateMachine.Actions;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Runtime
{
    public class StateMono : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private List<ActionBase> beforeEnter = new();
        [SerializeField] private List<ActionBase> onEnter     = new();
        [SerializeField] private List<ActionBase> onStay      = new();
        [SerializeField] private List<ActionBase> onLeave     = new();
        [SerializeField] private List<TransitionMono> transitions = new();

        public string Id => id;

        public string OnBeforeEnterAndCheckImmediate()
        {
            foreach (var a in beforeEnter) a.Execute();
            return TryGetValidTransition();
        }
        public void OnEnter() { foreach (var a in onEnter) a.Execute(); }
        public void Tick   () { foreach (var a in onStay)  a.Execute(); }
        public void OnLeave() { foreach (var a in onLeave) a.Execute(); }

        public string TryGetValidTransition()
        {
            foreach (var t in transitions)
                if (t.IsValid()) return t.ToId;
            return null;
        }

        public static async Task<StateMono> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var id = (string)node.Attribute("id");
            var go = new GameObject($"State_{id}");
            go.transform.SetParent(parent, false);
            var s = go.AddComponent<StateMono>();
            s.id = id;

            // ===== grupos de ações (cada um com seu GameObject) =====
            async Task AddGroupAsync(string name, XElement groupNode, List<ActionBase> target)
            {
                var groupGo = new GameObject(name);
                groupGo.transform.SetParent(go.transform, false);
                if (groupNode == null) return;

                var created = await Task.WhenAll(
                    groupNode.Elements().Select(n => ActionFactory.CreateAsync(n, groupGo.transform, player))
                );
                foreach (var a in created) if (a != null) target.Add(a);
            }

            await AddGroupAsync("BeforeEnter", node.Element("BeforeEnter"), s.beforeEnter);
            await AddGroupAsync("OnEnter",     node.Element("OnEnter"),     s.onEnter);
            await AddGroupAsync("OnStay",      node.Element("OnStay"),      s.onStay);
            await AddGroupAsync("OnLeave",     node.Element("OnLeave"),     s.onLeave);

            // ===== agrupador de transições =====
            var transitionsNode = node.Element("Transitions");
            if (transitionsNode != null)
            {
                // cria o GO pai "Transitions"
                var transitionsParent = new GameObject("Transitions");
                transitionsParent.transform.SetParent(go.transform, false);

                var created = await Task.WhenAll(
                    transitionsNode.Elements("Transition")
                        .Select(n => TransitionMono.ConstructFromXmlAsync(n, transitionsParent.transform, player))
                );
                foreach (var t in created) if (t != null) s.transitions.Add(t);
            }

            return s;
        }

        
    }
}
