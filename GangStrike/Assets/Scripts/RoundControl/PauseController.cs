using UnityEngine;

namespace RoundControl
{
    public class PauseController : MonoBehaviour
    {
        private bool isPaused = false;
        
        [SerializeField] private GameObject pauseMenuUI;
        private void Start()
        {
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);
            Time.timeScale = 1f; // Ensure time scale is normal at start
        }
        public void TogglePause()
        {
            isPaused = !isPaused;
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
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
