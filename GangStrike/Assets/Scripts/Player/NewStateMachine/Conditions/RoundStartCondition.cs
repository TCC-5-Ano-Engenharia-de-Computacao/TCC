using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine.Serialization;


namespace Player.NewStateMachine.Conditions
{
    using StateMachine;
    using UnityEngine;
    
    public sealed class RoundStartCondition: ConditionBase
    {
        [SerializeField] private GameRoot gameRoot;
        public override bool Evaluate()
        {
            if(gameRoot.roundController == null) 
                Debug.LogError("RoundStartCondition: RoundController is null");
            return gameRoot.roundController.roundStart.IsRoundStarting();
        }
        
        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(RoundStartCondition));
            go.transform.SetParent(parent, false);

            var cond = go.AddComponent<RoundStartCondition>();
            cond.gameRoot = Object.FindObjectOfType<GameRoot>();
            return Task.FromResult<ConditionBase>(cond);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(RoundStartCondition), ConstructFromXmlAsync);
    }
    
}