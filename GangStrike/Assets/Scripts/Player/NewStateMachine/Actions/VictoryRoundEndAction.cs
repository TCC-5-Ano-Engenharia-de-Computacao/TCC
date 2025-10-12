using DefaultNamespace;

namespace Player.NewStateMachine.Actions
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    namespace Player.NewStateMachine.Actions
    {
        public sealed class VictoryRoundEndAction : ActionBase
        {
            PlayerRoot player;
            GameRoot gameRoot;

            public override void Execute()
            {
                gameRoot.RoundEnd(player.GetComponentInChildren<PlayerAttributeController>().GetPlayerId());
            }

            public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
            {
                var go = new GameObject(nameof(VictoryRoundEndAction));
                go.transform.SetParent(parent, false);

                var action = go.AddComponent<VictoryRoundEndAction>();
                action.player = player;
                action.gameRoot = FindFirstObjectByType<GameRoot>();
                
                return action;      
            }

            [RuntimeInitializeOnLoadMethod]
            private static void Register() =>
                ActionFactory.Register(nameof(VictoryRoundEndAction), ConstructFromXmlAsync);
        }
    }

}