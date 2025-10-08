using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerText : MonoBehaviour
    {
        private CountdownTimer timerController;
        private TextMeshProUGUI timerText;

        private void Awake()
        {
            // Find TimerController in scene
            timerController = FindFirstObjectByType<CountdownTimer>();
            if (timerController == null)
            {
                Debug.LogError("TimerController not found in the scene!");
                enabled = false;
                return;
            }

            // Get reference to the TextMeshProUGUI component
            timerText = GetComponent<TextMeshProUGUI>();
            if (timerText == null)
            {
                Debug.LogError("No TextMeshProUGUI component found on this GameObject.");
                enabled = false;
            }
        }

        private void OnEnable()
        {
            if (timerController != null)
                timerController.timerUpdatedEvent.AddListener(UpdateTimerText);
        }

        private void OnDisable()
        {
            if (timerController != null)
                timerController.timerUpdatedEvent.RemoveListener(UpdateTimerText);
        }

        private void UpdateTimerText(float timeRemaining)
        {
            int seconds = Mathf.FloorToInt(timeRemaining);

            timerText.text = $"{seconds:00}";
        }
    }
}