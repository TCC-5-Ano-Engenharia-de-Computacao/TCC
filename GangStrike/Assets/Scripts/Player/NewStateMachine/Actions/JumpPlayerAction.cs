// Player.NewStateMachine.Actions.JumpPlayerAction.cs
namespace Player.NewStateMachine.Actions
{
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    public sealed class JumpPlayerAction : ActionBase
    {
        [SerializeField] private float force = 5f;
        [SerializeField] private new Rigidbody2D rigidbody2D;

        public override void Execute()
        {
            // zera a velocidade vertical antes do pulo (mantém o X)
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocityX, 0f);

            rigidbody2D.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(JumpPlayerAction));
            go.transform.SetParent(parent, false);

            var action = go.AddComponent<JumpPlayerAction>();

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
            ActionFactory.Register(nameof(JumpPlayerAction), ConstructFromXmlAsync);
    }
}