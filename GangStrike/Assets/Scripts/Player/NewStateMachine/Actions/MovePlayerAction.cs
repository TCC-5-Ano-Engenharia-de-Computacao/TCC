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
            [SerializeField] private SideSwapper sideSwapper;

            public override void Execute()
            {
                rigidbody2D.linearVelocity = new Vector2(dirSign * speed * (sideSwapper.swapped ? -1 : 1), rigidbody2D.linearVelocityY);
            }

            public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
            {
                var go = new GameObject(nameof(MovePlayerAction));
                go.transform.SetParent(parent, false);

                var action = go.AddComponent<MovePlayerAction>();
                
                action.rigidbody2D = player.characterRoot.rigidbody2D;
                action.sideSwapper = player.characterRoot.sideSwapper;
                
                action.speed =
                    float.TryParse((string)node.Attribute("speed"),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var spd)
                        ? spd : action.speed;

                // direction="forward" | "backward"
                action.dirSign =
                    (((string)node.Attribute("direction"))?
                        .Trim()
                        .StartsWith("f", StringComparison.OrdinalIgnoreCase) ?? false) // forward
                        ? 1 : -1;

                
                return action;      
            }

            [RuntimeInitializeOnLoadMethod]
            private static void Register() =>
                ActionFactory.Register(nameof(MovePlayerAction), ConstructFromXmlAsync);
        }
    }

}