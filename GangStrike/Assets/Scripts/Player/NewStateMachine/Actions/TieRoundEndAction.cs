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
        public sealed class TieRoundEndAction : ActionBase
        {
            GameRoot gameRoot;

            public override void Execute()
            {
                gameRoot.TieRoundEnd();
            }

            public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
            {
                var go = new GameObject(nameof(TieRoundEndAction));
                go.transform.SetParent(parent, false);

                var action = go.AddComponent<TieRoundEndAction>();
                action.gameRoot = FindFirstObjectByType<GameRoot>();
                
                return action;      
            }

            [RuntimeInitializeOnLoadMethod]
            private static void Register() =>
                ActionFactory.Register(nameof(TieRoundEndAction), ConstructFromXmlAsync);
        }
    }

}