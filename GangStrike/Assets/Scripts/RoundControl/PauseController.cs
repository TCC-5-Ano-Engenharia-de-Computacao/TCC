using StateMachine;
using UnityEngine;

namespace RoundControl
{
    public class PauseController : MonoBehaviour
    {
        private bool isPaused = false;
        private PlayerRoot[] players;
        private GameRoot gameRoot;
        
        [SerializeField] private GameObject pauseMenuUI;
        private void Start()
        {
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);
            Time.timeScale = 1f; // Ensure time scale is normal at start
        }
        public void TogglePause()
        {
            gameRoot = FindFirstObjectByType<GameRoot>();
            players = gameRoot != null ? gameRoot.GetPlayers() : null;
            isPaused = !isPaused;
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
            if (players != null)
                foreach (PlayerRoot player in players)
                {
                    if (player == null)
                    {
                        Debug.LogError("PauseController: One of the players is null.");
                        return;
                    }
                    else
                    {
                        if(isPaused)
                            player.inputRoot.inputController.ForceDisablePlayerInput();
                        else
                            player.inputRoot.inputController.ForceEnablePlayerInput();
                    }
                }
        }

        public void Pause()
        {
            isPaused = true;
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f; // Ensure time scale is reset when the controller is destroyed
        }
    }
}
