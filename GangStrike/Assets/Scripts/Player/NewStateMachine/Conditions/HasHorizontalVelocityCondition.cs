// Player.NewStateMachine.Conditions.HorizontalMovementCondition.cs

using System.Globalization;

namespace Player.NewStateMachine.Conditions
{
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    public sealed class HasHorizontalVelocityCondition : ConditionBase
    {
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private float speedThreshold = 0.1f;
        [SerializeField] private bool invertCondition = false;

        public override bool Evaluate()
        {
            var vel = rb2D.linearVelocityX;
            return (vel > speedThreshold || vel < -speedThreshold) != invertCondition;
        }

        public static async Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(HasHorizontalVelocityCondition));
            go.transform.SetParent(parent, false);

            var c = go.AddComponent<HasHorizontalVelocityCondition>();
            c.rb2D = player.characterRoot.rigidbody2D;
            
            c.speedThreshold =
                float.TryParse((string)node.Attribute("speedThreshold"),
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var spd)
                    ? spd : c.speedThreshold;
            
            var attr = (string?)node.Attribute("invertCondition");
            if (!string.IsNullOrEmpty(attr) && bool.TryParse(attr, out var hold))
                c.invertCondition = hold;

            return c;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(HasHorizontalVelocityCondition), ConstructFromXmlAsync);
    }
}