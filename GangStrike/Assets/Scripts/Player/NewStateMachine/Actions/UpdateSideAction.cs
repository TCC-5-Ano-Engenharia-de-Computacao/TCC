namespace Player.NewStateMachine.Actions
{
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

    namespace Player.NewStateMachine.Actions
    {
        public sealed class UpdateSideAction : ActionBase
        {
            [SerializeField] private SideSwapper sideSwapper;

            public override void Execute()
            {
                sideSwapper.UpdateSideSwap();
            }

            public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
            {
                var go = new GameObject(nameof(UpdateSideAction));
                go.transform.SetParent(parent, false);

                var action = go.AddComponent<UpdateSideAction>();
                
                action.sideSwapper = player.characterRoot.sideSwapper;
                
                return action;      
            }

            [RuntimeInitializeOnLoadMethod]
            private static void Register() =>
                ActionFactory.Register(nameof(UpdateSideAction), ConstructFromXmlAsync);
        }
    }

}