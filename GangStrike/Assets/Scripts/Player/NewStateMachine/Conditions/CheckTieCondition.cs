using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine.Serialization;

namespace Player.NewStateMachine.Conditions
{
    using StateMachine;
    using UnityEngine;
    
    public sealed class CheckTieCondition: ConditionBase
    {
        [SerializeField] private PlayerRoot playerRoot; 
        [SerializeField] private GameRoot gameRoot;

        public bool CheckTimerEndTieCondition()
        {
            bool isTimerEnded = !gameRoot.countdownTimer.IsTimerRunning();
            bool hasEqualHealth = Mathf.Approximately(playerRoot.attributeController.AttributeSystem
                .HealthValue, gameRoot.GetEnemyPlayer(playerRoot)
                .attributeController.AttributeSystem.HealthValue);
            return isTimerEnded && hasEqualHealth;
        }
        
        public bool CheckBothPlayersDeadCondition()
        {
            bool isPlayerDead = !playerRoot.attributeController.AttributeSystem.IsAlive();
            bool isEnemyDead = !gameRoot.GetEnemyPlayer(playerRoot)
                .attributeController.AttributeSystem.IsAlive();
            return isPlayerDead && isEnemyDead;
        }
        
        public override bool Evaluate()
        {
            return CheckTimerEndTieCondition() || CheckBothPlayersDeadCondition();
        }
        
        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(CheckTieCondition));
            go.transform.SetParent(parent, false);

            var cond = go.AddComponent<CheckTieCondition>();
            cond.playerRoot = player;
            cond.gameRoot = Object.FindObjectOfType<GameRoot>();
            if(cond.gameRoot == null)
                Debug.LogError("CheckTieCondition: GameRoot not found in scene.");
            return Task.FromResult<ConditionBase>(cond);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(CheckTieCondition), ConstructFromXmlAsync);
    }
    
}