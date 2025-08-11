// LogAction.cs

using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;
// ajuste namespace

namespace Player.NewStateMachine.Actions
{
    public sealed class LogAction : ActionBase
    {
        [SerializeField] private string message;

        public override void Execute() => Debug.Log(message);

        public static Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot _)
        {
            var go = new GameObject("LogAction");
            go.transform.SetParent(parent, false);

            var a = go.AddComponent<LogAction>();
            a.message = (string)node.Attribute("msg") ?? string.Empty;

            return Task.FromResult<ActionBase>(a);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() => ActionFactory.Register("LogAction", ConstructFromXmlAsync);
    }
}