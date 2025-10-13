using System.Collections;
using UnityEngine;

namespace RoundControl
{
    public class RoundStart : MonoBehaviour
    {
        private GameRoot gameRoot;
        private bool isRoundStarting;
        [SerializeField] private TMPro.TextMeshProUGUI startText;
        public float fadeDuration = 0.3f;          // Duration for fade in/out
        [SerializeField]private string[] countdownMessages = new string[] { "3", "2", "1", "FIGHT!" }; // Countdown sequence
        private void Awake()
        {
            gameRoot = FindFirstObjectByType<GameRoot>();
            isRoundStarting = false;
        }

        private void Start()
        {
            startText.enabled = false;
            StartRound();
        }
        
        private void StartRound()
        {
            isRoundStarting = false;
            StartCoroutine(ShowRoundStartBanner());
        }

        IEnumerator ShowRoundStartBanner()
        {
            startText.enabled = true;
            gameRoot.countdownTimer.StopTimer();
            yield return StartCoroutine(CountdownSequence());
            gameRoot.countdownTimer.StartTimer();
            isRoundStarting = true;
            startText.enabled = false;
        }
        
        private IEnumerator CountdownSequence()
        {
            foreach (var message in countdownMessages)
            {
                yield return StartCoroutine(FadeText(message));
            }
        }

        private IEnumerator FadeText(string message)
        {
            // Fade in
            startText.text = message;
            float currentAlpha = startText.color.a;
            float targetAlpha = 1f;
            float startTime = Time.time;

            // Fade in the text
            while (Time.time - startTime < fadeDuration)
            {
                float lerpValue = (Time.time - startTime) / fadeDuration;
                float alpha = Mathf.Lerp(currentAlpha, targetAlpha, lerpValue);
                startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, alpha);
                yield return null;
            }

            // Ensure the text is fully visible after fade-in
            startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, targetAlpha);

            // Wait for a moment before starting fade out
            yield return new WaitForSeconds(0.5f);

            // Fade out
            currentAlpha = startText.color.a;
            targetAlpha = 0f;
            startTime = Time.time;

            // Fade out the text
            while (Time.time - startTime < fadeDuration)
            {
                float lerpValue = (Time.time - startTime) / fadeDuration;
                float alpha = Mathf.Lerp(currentAlpha, targetAlpha, lerpValue);
                startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, alpha);
                yield return null;
            }

            // Ensure the text is fully transparent after fade-out
            startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, targetAlpha);
        }
        
        public bool IsRoundStarting()
        {
            return isRoundStarting;
        }
    }
}
