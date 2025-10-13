using RoundControl;
using StateMachine;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameRoot: MonoBehaviour
{
    [SerializeField] private int roundsToWin = 2;
    [SerializeField] private int currentRound = 1;
    public UnityEvent roundEndedEvent;
    public UnityEvent<int> gameEndedEvent;
    public UnityEvent<Vector2> scoreUpdatedEvent;
    [SerializeField] private Vector2 score = Vector2.zero; // x = player 1 score, y = player 2 score
    public UI.CountdownTimer countdownTimer;
    public RoundController roundController;
    [SerializeField][ReadOnly]private PlayerRoot[] players;

        
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void RegisterPlayers(PlayerRoot[] playerRoots)
    {
        if (playerRoots == null || playerRoots.Length != 2)
        {
            Debug.LogError("RegisterPlayers: Invalid player roots array.");
            return;
        }
        foreach (var player in playerRoots)
        {
            if (player == null)
            {
                Debug.LogError("RegisterPlayers: One of the player roots is null.");
                return;
            }
        }
        players = playerRoots;
    }
        
    public void RegisterCountdownTimer(UI.CountdownTimer timer)
    {
        if (timer == null)
        {
            Debug.LogError("RegisterCountdownTimer: Timer is null.");
            return;
        }
        countdownTimer = timer;
    }
        
    public void RegisterRoundController(RoundController controller)
    {
        if (controller == null)
        {
            Debug.LogError("RegisterRoundController: Controller is null.");
            return;
        }
        roundController = controller;
    }
        
    public PlayerRoot GetEnemyPlayer(PlayerRoot requestingPlayer)
    {
        if (players == null || players.Length != 2)
        {
            Debug.LogError("GetEnemyPlayer: Players not initialized or invalid number of players.");
            return null;
        }
        PlayerRoot otherPlayer = players[0] == requestingPlayer
            ? players[1]
            : players[0];
        return otherPlayer;
    }
        
    public void RoundEnd(int winningPlayerId)
    {
        // Updates score, checks for game end, if not game end, then round end
        score = winningPlayerId == 1 ? new Vector2(score.x + 1, score.y) : new Vector2(score.x, score.y + 1);
        scoreUpdatedEvent?.Invoke(score);
        if(score.x >= roundsToWin || score.y >= roundsToWin)
        {
            // Game End
            gameEndedEvent?.Invoke(winningPlayerId);
            Debug.Log($"Game End! Final Score: Player 1: {score.x} - Player 2: {score.y}");
        }
        else
        {
            // Round End
            roundEndedEvent?.Invoke();
            Debug.Log($"Round End! Current Score: Player 1: {score.x} - Player 2: {score.y}");
        }
    }
        
    public int CurrentRound => currentRound;
    public Vector2 GetScore() => score;
    public void ResetEverything()
    {
        score = Vector2.zero;
        currentRound = 1;
    }
}