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
        [SerializeField] private BodyColliders enemyBodyColliders;
        [SerializeField] private IncomingHitBuffer.Hit hit;
        [SerializeField] private IncomingHitBuffer enemyHitBuffer;

        public override void Execute()
        {
            if (attackCollider.IsTouching(enemyBodyColliders.GetActiveCollider()))
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
            
            a.attackCollider = player.characterRoot.attackTriggers.GetAttackColliderByName(a.hit.tag);

            var enemyCharacterRoot = GameObject.FindFirstObjectByType<GameRoot>().GetEnemyPlayer(player).characterRoot;
            
            a.enemyHitBuffer = enemyCharacterRoot.incomingHitBuffer;
            a.enemyBodyColliders = enemyCharacterRoot.bodyColliders;
            
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