using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Actions
{
    public class SpawnEnergyBeamAction : ActionBase
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private CharacterRoot attackerCharacterRoot;
        [SerializeField] private IncomingHitBuffer.Hit hit;
        [SerializeField] private GameObject energyBeamPrefab;
        [SerializeField] private float speed = 5f;

        public override void Execute()
        {
            GameObject beam = Instantiate(energyBeamPrefab, spawnPoint.position, attackerCharacterRoot.transform.rotation);
            beam.GetComponent<EnergyBeam>().InitializeEnergyBeam(attackerCharacterRoot, hit, speed);
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(SpawnEnergyBeamAction));
            go.transform.SetParent(parent, false);

            var a = go.AddComponent<SpawnEnergyBeamAction>();
            
            a.hit = new IncomingHitBuffer.Hit(
                damage: ConvertStrToFloat((string)node.Attribute("damage")),
                effect: (string)node.Attribute("effect"),
                tag: (string)node.Attribute("tag"),
                sameAttackCooldown: ConvertStrToFloat((string)node.Attribute("sameAttackCooldown")),
                genericCooldown: ConvertStrToFloat((string)node.Attribute("genericCooldown")),
                knockBackForce: ConvertStrToFloat((string)node.Attribute("knockBackForce")),
                stunDuration: ConvertStrToFloat((string)node.Attribute("stunDuration"))
            );
            
            a.speed = ConvertStrToFloat((string)node.Attribute("speed"));
            
            a.attackerCharacterRoot = player.characterRoot;
            a.spawnPoint = player.characterRoot.projectileSpawnPoint;
            a.energyBeamPrefab = player.characterRoot.energyBeamPrefab;
            
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
            ActionFactory.Register(nameof(SpawnEnergyBeamAction), ConstructFromXmlAsync);
    }
}