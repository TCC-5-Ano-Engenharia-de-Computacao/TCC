using System.Collections;
using UnityEngine;

namespace RoundControl
{
    public class RoundStart : MonoBehaviour
    {
        private GameRoot gameRoot;
        private bool isRoundStarting;

        private void Awake()
        {
            gameRoot = FindFirstObjectByType<GameRoot>();
            isRoundStarting = false;
        }

        private void Start()
        {
            StartRound();
        }
        
        private void StartRound()
        {
            isRoundStarting = false;
            StartCoroutine(ShowRoundStartBanner());
        }

        IEnumerator ShowRoundStartBanner()
        {
            gameRoot.countdownTimer.StopTimer();
            // Get the center of the screen in world coordinates
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);

            // Convert the screen center to world coordinates
            Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

            // Instantiate the prefab at the world center position
            // Uncomment the following line and replace 'prefab' with your actual prefab variable
            // Instantiate(prefab, worldCenter, Quaternion.identity);

            // Wait for 3 seconds, then return to menu
            yield return new WaitForSeconds(3f);
            gameRoot.countdownTimer.StartTimer();
            isRoundStarting = true;
        }
        
        public bool IsRoundStarting()
        {
            return isRoundStarting;
        }
    }
}
