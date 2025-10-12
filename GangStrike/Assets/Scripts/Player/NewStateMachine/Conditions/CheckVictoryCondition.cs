using System.Threading.Tasks;
using System.Xml.Linq;
using DefaultNamespace;
using UnityEngine.Serialization;

namespace Player.NewStateMachine.Conditions
{
    using StateMachine;
    using UnityEngine;
    
    public sealed class CheckVictoryCondition: ConditionBase
    {
        [SerializeField] private PlayerRoot playerRoot; 
        [SerializeField] private GameRoot gameRoot;

        public bool CheckTimerEndWinCondition()
        {
            bool isTimerEnded = !gameRoot.countdownTimer.IsTimerRunning();
            bool hasMoreHealth = playerRoot.attributeController.AttributeSystem
                .HealthValue > gameRoot.GetEnemyPlayer(playerRoot)
                .attributeController.AttributeSystem.HealthValue;
            return isTimerEnded && hasMoreHealth;
        }
        
        public override bool Evaluate()
        {
            
            return !gameRoot.GetEnemyPlayer(playerRoot).attributeController.AttributeSystem.IsAlive()||
                   CheckTimerEndWinCondition();
        }
        
        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(CheckVictoryCondition));
            go.transform.SetParent(parent, false);

            var cond = go.AddComponent<CheckVictoryCondition>();
            cond.playerRoot = player;
            cond.gameRoot = Object.FindObjectOfType<GameRoot>();
            return Task.FromResult<ConditionBase>(cond);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(CheckVictoryCondition), ConstructFromXmlAsync);
    }
    
}