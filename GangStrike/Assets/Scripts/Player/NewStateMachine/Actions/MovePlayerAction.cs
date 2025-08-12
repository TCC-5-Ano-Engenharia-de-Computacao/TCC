namespace Player.NewStateMachine.Actions
{
    // Player.NewStateMachine.Actions.MovePlayerAction.cs
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    namespace Player.NewStateMachine.Actions
    {
        public sealed class MovePlayerAction : ActionBase
        {
            [SerializeField] private float speed = 5f;
            [SerializeField] private int dirSign = 1;
            [SerializeField] private new Rigidbody2D rigidbody2D;

            public override void Execute()
            {
                rigidbody2D.linearVelocity = new Vector2(dirSign * speed, rigidbody2D.linearVelocityY);
            }

            public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
            {
                var go = new GameObject(nameof(MovePlayerAction));
                go.transform.SetParent(parent, false);

                var action = go.AddComponent<MovePlayerAction>();

                action.rigidbody2D = player.characterRoot.rigidbody2D;

                action.speed =
                    float.TryParse((string)node.Attribute("speed"),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var spd)
                        ? spd : action.speed;

                // direction="left" | "right"
                action.dirSign =
                    (((string)node.Attribute("direction"))?
                        .Trim()
                        .StartsWith("l", StringComparison.OrdinalIgnoreCase) ?? false)
                        ? -1 : 1;

                
                return action;      
            }

            [RuntimeInitializeOnLoadMethod]
            private static void Register() =>
                ActionFactory.Register(nameof(MovePlayerAction), ConstructFromXmlAsync);
        }
    }

}