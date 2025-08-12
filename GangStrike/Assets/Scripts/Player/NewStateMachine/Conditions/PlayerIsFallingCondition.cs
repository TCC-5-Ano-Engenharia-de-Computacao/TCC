// Player.NewStateMachine.Conditions.PlayerIsFallingCondition.cs
namespace Player.NewStateMachine.Conditions
{
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    public sealed class PlayerIsFallingCondition : ConditionBase
    {
        [SerializeField] private new Rigidbody2D rigidbody2D;

        public override bool Evaluate() =>
            rigidbody2D && rigidbody2D.linearVelocityY < -0.01f;

        public static async Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(PlayerIsFallingCondition));
            go.transform.SetParent(parent, false);

            var c = go.AddComponent<PlayerIsFallingCondition>();
            c.rigidbody2D = player.characterRoot.rigidbody2D;

            return c;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(PlayerIsFallingCondition), ConstructFromXmlAsync);
    }
}