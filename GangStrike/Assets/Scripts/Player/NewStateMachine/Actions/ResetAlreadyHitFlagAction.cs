using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Actions
{
    public class ResetAlreadyHitFlagAction : ActionBase
    {
        [SerializeField] private AttackTriggers attackTriggers;

        public override void Execute()
        {
            attackTriggers.alreadyHit = false;
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(ResetAlreadyHitFlagAction));
            go.transform.SetParent(parent, false);

            var a = go.AddComponent<ResetAlreadyHitFlagAction>();
            
            a.attackTriggers = player.characterRoot.attackTriggers;
            
            return a;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ActionFactory.Register(nameof(ResetAlreadyHitFlagAction), ConstructFromXmlAsync);
    }
}