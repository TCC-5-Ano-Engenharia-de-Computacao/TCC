using System.Collections;
using TMPro;
using UnityEngine;

namespace RoundControl
{
    public class RoundEnd : MonoBehaviour
    {
        private RoundController roundController;
        private GameRoot gameRoot;
        [SerializeField] private TextMeshProUGUI endRoundText;
        public string textToType = "K.O";        // Text you want to type out (e.g., "K.O")
        public float typingSpeed = 0.4f;         // Time delay between each character being typed
        public float fadeDuration = 0.3f;          // Duration of the fade-out effect after typing is done

        
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
            endRoundText.enabled = true;
            yield return StartCoroutine(TypeAndFadeText());
            roundController.roundResetEvent.Invoke();
        }
        
        private IEnumerator TypeAndFadeText()
        {
            endRoundText.text = "";  // Clear the text at the beginning

            // Type out each character
            foreach (char letter in textToType)
            {
                endRoundText.text += letter;  // Add one character at a time
                yield return new WaitForSeconds(typingSpeed);  // Wait before typing the next character
            }

            // After typing is done, start fading out the text
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(FadeOutText());
        }

        private IEnumerator FadeOutText()
        {
            float startAlpha = endRoundText.color.a;
            float endAlpha = 0f;
            float startTime = Time.time;

            // Fade out over 'fadeDuration' seconds
            while (Time.time - startTime < fadeDuration)
            {
                float lerpValue = (Time.time - startTime) / fadeDuration;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, lerpValue);
                endRoundText.color = new Color(endRoundText.color.r, endRoundText.color.g, endRoundText.color.b, alpha);
                yield return null;
            }

            // Ensure the text is fully transparent after fade-out
            endRoundText.color = new Color(endRoundText.color.r, endRoundText.color.g, endRoundText.color.b, endAlpha);
        }
    }
}
