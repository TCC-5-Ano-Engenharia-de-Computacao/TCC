// Player.NewStateMachine.Conditions.PlayerIsGroundedCondition.cs
namespace Player.NewStateMachine.Conditions
{
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    public sealed class PlayerIsGroundedCondition : ConditionBase
    {
        [SerializeField] private Collider2D footGroundCollider;
        [SerializeField] private float checkDistance = 0.05f;
        [SerializeField] private ContactFilter2D contactFilter;

        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];

        public override bool Evaluate()
        {
            int hitCount = footGroundCollider.Cast(Vector2.down, contactFilter, _hits, checkDistance);

            return hitCount > 0;
        }

        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(PlayerIsGroundedCondition));
            go.transform.SetParent(parent, false);

            var c = go.AddComponent<PlayerIsGroundedCondition>();
            c.footGroundCollider = player.characterRoot.footGroundCollider;

            // Você pode ajustar o filtro aqui se quiser ignorar certas layers
            c.contactFilter = new ContactFilter2D
            {
                useTriggers = false
                
            };
            c.contactFilter.SetLayerMask(Physics2D.DefaultRaycastLayers);

            return Task.FromResult<ConditionBase>(c);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(PlayerIsGroundedCondition), ConstructFromXmlAsync);
    }
}