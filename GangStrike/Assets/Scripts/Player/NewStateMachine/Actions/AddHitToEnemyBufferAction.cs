using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Actions
{
    public class AddHitToEnemyBufferAction : ActionBase
    {
        [SerializeField] private Collider2D attackCollider;
        [SerializeField] private Collider2D enemyCollider;
        [SerializeField] private IncomingHitBuffer enemyHitBuffer;
        [SerializeField] private IncomingHitBuffer.Hit hit;

        public override void Execute()
        {
            if (attackCollider.IsTouching(enemyCollider))
            {
                //Debug.Log("HIT!");
                enemyHitBuffer.AddHitToBuffer(hit);
            }
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(AddHitToEnemyBufferAction));
            go.transform.SetParent(parent, false);

            var a = go.AddComponent<AddHitToEnemyBufferAction>();
            
            a.hit = new IncomingHitBuffer.Hit(
                damage: ConvertStrToFloat((string)node.Attribute("damage")),
                effect: (string)node.Attribute("effect"),
                tag: (string)node.Attribute("tag"),
                sameAttackCooldown: ConvertStrToFloat((string)node.Attribute("sameAttackCooldown")),
                genericCooldown: ConvertStrToFloat((string)node.Attribute("genericCooldown")),
                knockBackForce: ConvertStrToFloat((string)node.Attribute("knockBackForce")),
                stunDuration: ConvertStrToFloat((string)node.Attribute("stunDuration"))
            );
            
            a.attackCollider = player.characterRoot.attackCollider;

            var allPlayers = GameObject.FindObjectsByType<PlayerRoot>(FindObjectsSortMode.None);
            foreach (PlayerRoot e in allPlayers)
            {
                if (e != player)
                {
                    a.enemyCollider = e.characterRoot.bodyCollider;
                    a.enemyHitBuffer = e.characterRoot.incomingHitBuffer;
                    break;
                }
            }
            
            return a;
        }

        static float ConvertStrToFloat(string str)
        {
            return float.TryParse(str,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var converted)
                ? converted
                : 0f;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ActionFactory.Register(nameof(AddHitToEnemyBufferAction), ConstructFromXmlAsync);
    }
}