// Player.NewStateMachine.Actions.KnockbackAction.cs
namespace Player.NewStateMachine.Actions
{
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    public sealed class KnockbackAction : ActionBase
    {
        [SerializeField] private float force = 5f;
        [SerializeField] private new Rigidbody2D rigidbody2D;

        public override void Execute()
        {
            // zera a velocidade vertical antes do pulo (mantém o X)
            //rigidbody2D.linearVelocity = Vector2.zero;

            rigidbody2D.AddForce(- rigidbody2D.transform.right * force, ForceMode2D.Impulse);
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(KnockbackAction));
            go.transform.SetParent(parent, false);

            var action = go.AddComponent<KnockbackAction>();

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

            return action;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ActionFactory.Register(nameof(KnockbackAction), ConstructFromXmlAsync);
    }
}