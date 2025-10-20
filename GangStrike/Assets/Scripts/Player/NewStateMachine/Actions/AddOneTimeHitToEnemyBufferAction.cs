using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Actions
{
    public class AddOneTimeHitToEnemyBufferAction : ActionBase
    {
        [SerializeField] private AttackTriggers attackTriggers;
        [SerializeField] private Collider2D thisAttackTrigger;
        [SerializeField] private AttributeSystem attackerAttributeSystem;
        [SerializeField] private BodyColliders enemyBodyColliders;
        [SerializeField] private IncomingHitBuffer.Hit hit;
        [SerializeField] private IncomingHitBuffer enemyHitBuffer;

        public override void Execute()
        {
            if (!attackTriggers.alreadyHit)
            {
                if (thisAttackTrigger.IsTouching(enemyBodyColliders.GetActiveCollider()))
                {
                    //Debug.Log("HIT!");
                    attackerAttributeSystem.GainUltimate(hit.damage * 0.4f);
                    enemyHitBuffer.AddHitToBuffer(hit);
                    attackTriggers.alreadyHit = true;
                }
            }
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(AddOneTimeHitToEnemyBufferAction));
            go.transform.SetParent(parent, false);

            var a = go.AddComponent<AddOneTimeHitToEnemyBufferAction>();
            
            a.hit = new IncomingHitBuffer.Hit(
                damage: ConvertStrToFloat((string)node.Attribute("damage")),
                effect: (string)node.Attribute("effect"),
                tag: (string)node.Attribute("tag"),
                sameAttackCooldown: ConvertStrToFloat((string)node.Attribute("sameAttackCooldown")),
                genericCooldown: ConvertStrToFloat((string)node.Attribute("genericCooldown")),
                knockBackForce: ConvertStrToFloat((string)node.Attribute("knockBackForce")),
                stunDuration: ConvertStrToFloat((string)node.Attribute("stunDuration"))
            );

            a.attackTriggers = player.characterRoot.attackTriggers;
            a.thisAttackTrigger = a.attackTriggers.GetAttackColliderByName(a.hit.tag);
            a.attackerAttributeSystem = player.attributeSystem;
            
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
            ActionFactory.Register(nameof(AddOneTimeHitToEnemyBufferAction), ConstructFromXmlAsync);
    }
}