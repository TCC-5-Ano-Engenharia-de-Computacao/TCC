using System.Collections;
using UnityEngine;

namespace RoundControl
{
    public class RoundEnd : MonoBehaviour
    {
        private RoundController roundController;
        private GameRoot gameRoot;
        
        private void Awake()
        {
            gameRoot = FindFirstObjectByType<GameRoot>();
            roundController = GetComponent<RoundController>();
        }

        private void OnEnable()
        {
            if (gameRoot != null)
            {
                gameRoot.roundEndedEvent.AddListener(OnRoundEnd);
            }
        }

        private void OnRoundEnd()
        {
            StartCoroutine(ShowEndRoundBanner());
        }
        
        IEnumerator ShowEndRoundBanner()
        {
            gameRoot.countdownTimer.StopTimer();
            // Get the center of the screen in world coordinates
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);

            // Convert the screen center to world coordinates
            Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

            // Instantiate the prefab at the world center position
            // Uncomment the following line and replace 'prefab' with your actual prefab variable
            // Instantiate(prefab, worldCenter, Quaternion.identity);

            // Wait for 3 seconds, then reset the round
            yield return new WaitForSeconds(3f);
            roundController.roundResetEvent.Invoke();
        }
    }
}
