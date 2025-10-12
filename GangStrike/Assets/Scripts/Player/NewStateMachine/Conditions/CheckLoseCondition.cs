using System.Threading.Tasks;
using System.Xml.Linq;
using DefaultNamespace;
using UnityEngine.Serialization;

namespace Player.NewStateMachine.Conditions
{
    using StateMachine;
    using UnityEngine;
    
    public sealed class CheckLoseCondition: ConditionBase
    {
        [SerializeField] private PlayerRoot playerRoot; 
        [SerializeField] private GameRoot gameRoot;

        public bool CheckTimerEndLoseCondition()
        {
            bool isTimerEnded = !gameRoot.countdownTimer.IsTimerRunning();
            bool hasLessHealth = playerRoot.attributeController.AttributeSystem
                .HealthValue <= gameRoot.GetEnemyPlayer(playerRoot)
                .attributeController.AttributeSystem.HealthValue;
            return isTimerEnded && hasLessHealth;
        }
        
        public override bool Evaluate()
        {
            
            return !playerRoot.attributeController.AttributeSystem.IsAlive()||
                   CheckTimerEndLoseCondition();
        }
        
        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(CheckLoseCondition));
            go.transform.SetParent(parent, false);

            var cond = go.AddComponent<CheckLoseCondition>();
            cond.playerRoot = player;
            cond.gameRoot = Object.FindObjectOfType<GameRoot>();
            return Task.FromResult<ConditionBase>(cond);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(CheckLoseCondition), ConstructFromXmlAsync);
    }
    
}