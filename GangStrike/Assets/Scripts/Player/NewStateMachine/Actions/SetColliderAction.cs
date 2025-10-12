using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using DefaultNamespace;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Actions
{
    public class SetColliderAction : ActionBase
    {
        [SerializeField] private string colliderName;
        [SerializeField] private BodyColliders bodyColliders;

        public override void Execute()
        {
            bodyColliders.SetActiveCollider(colliderName);
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(SetColliderAction));
            go.transform.SetParent(parent, false);

            var a = go.AddComponent<SetColliderAction>();
            
            a.colliderName = (string)node.Attribute("colliderName");

            a.bodyColliders = player.characterRoot.bodyColliders;
            
            return a;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ActionFactory.Register(nameof(SetColliderAction), ConstructFromXmlAsync);
    }
}