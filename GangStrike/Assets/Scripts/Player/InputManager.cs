using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private GameRoot gameRoot;
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;
    [SerializeField] private Transform player1SpawnPoint;
    [SerializeField] private Transform player2SpawnPoint;

    void Awake()
    {
        gameRoot = FindFirstObjectByType<GameRoot>();
        var player1 = PlayerInput.Instantiate(player1Prefab, controlScheme: "WASD", pairWithDevice: Keyboard.current);
        player1.transform.position = player1SpawnPoint.position;

        var player2 = PlayerInput.Instantiate(player2Prefab, controlScheme: "Arrows", pairWithDevice: Keyboard.current);
        player2.transform.position = player2SpawnPoint.position;

        var playerRoot = player1.transform.root.GetComponentInChildren<PlayerRoot>();
        var playerRoot2 = player2.transform.root.GetComponentInChildren<PlayerRoot>();
        gameRoot.RegisterPlayers(new[] { playerRoot, playerRoot2 });
    }
}
