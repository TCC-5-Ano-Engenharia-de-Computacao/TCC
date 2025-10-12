using StateMachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameRoot: MonoBehaviour
    {
        [SerializeField] public UI.CountdownTimer countdownTimer;
        private PlayerRoot[] players;
        
        private void Start()
        {
            players = FindObjectsByType<PlayerRoot>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }
        
        public PlayerRoot GetEnemyPlayer(PlayerRoot requestingPlayer)
        {
            PlayerRoot otherPlayer = players[0] == requestingPlayer
                ? players[1]
                : players[0];
            return otherPlayer;
        }
        
    }
}