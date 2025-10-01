using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Actions
{
    public class ClearHitBufferAction : ActionBase
    {
        [SerializeField] private IncomingHitBuffer hitBuffer;

        public override void Execute()
        {
            hitBuffer.ClearHitBuffer();
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(ClearHitBufferAction));
            go.transform.SetParent(parent, false);

            var a = go.AddComponent<ClearHitBufferAction>();
            
            a.hitBuffer = player.characterRoot.incomingHitBuffer;
            
            return a;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ActionFactory.Register(nameof(ClearHitBufferAction), ConstructFromXmlAsync);
    }
}