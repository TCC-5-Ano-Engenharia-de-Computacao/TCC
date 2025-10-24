// Player.NewStateMachine.Actions.KnockbackAction.cs
namespace Player.NewStateMachine.Actions
{
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    public sealed class AvoidHeadAction : ActionBase
    {
        [SerializeField] private float force = 5f;
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private Collider2D footGroundCollider;
        [SerializeField] private float checkDistance = 0.05f;
        [SerializeField] private ContactFilter2D contactFilter;
        [SerializeField] private SideSwapper sideSwapper;
        
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];

        public override void Execute()
        {
            if (footGroundCollider.Cast(Vector2.down, contactFilter, _hits, checkDistance) > 0)
            {
                sideSwapper.UpdateSideSwap();
                rigidbody2D.AddForce(-rigidbody2D.transform.right * force, ForceMode2D.Impulse);
            }
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(AvoidHeadAction));
            go.transform.SetParent(parent, false);

            var action = go.AddComponent<AvoidHeadAction>();
            
            action.sideSwapper = player.characterRoot.sideSwapper;
            
            action.footGroundCollider = player.characterRoot.footGroundCollider;

            // fonte da verdade do RB2D vem do player
            action.rigidbody2D = player.characterRoot.rigidbody2D;

            // force="8.5" (opcional; default = 5f)
            if (float.TryParse((string)node.Attribute("force"),
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var parsed))
            {
                action.force = parsed;
            }
            
            action.contactFilter = new ContactFilter2D
            {
                useTriggers = false
            };
            
            action.contactFilter.SetLayerMask(LayerMask.GetMask("Player"));

            return action;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ActionFactory.Register(nameof(AvoidHeadAction), ConstructFromXmlAsync);
    }
}