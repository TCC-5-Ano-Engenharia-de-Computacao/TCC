// Player.NewStateMachine.Conditions.PlayerIsNotFallingCondition.cs
namespace Player.NewStateMachine.Conditions
{
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    public sealed class PlayerIsNotFallingCondition : ConditionBase
    {
        [SerializeField] private new Rigidbody2D rigidbody2D;

        public override bool Evaluate()
        {
            if (!rigidbody2D) return true; // se não tiver RB, considera não caindo
            return rigidbody2D.linearVelocityY >= -0.01f;
        }

        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(PlayerIsNotFallingCondition));
            go.transform.SetParent(parent, false);

            var c = go.AddComponent<PlayerIsNotFallingCondition>();
            c.rigidbody2D = player.characterRoot.rigidbody2D;

            return Task.FromResult<ConditionBase>(c);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(PlayerIsNotFallingCondition), ConstructFromXmlAsync);
    }
}
