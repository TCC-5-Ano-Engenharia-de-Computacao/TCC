using System.Collections;
using TMPro;
using UnityEngine;

namespace RoundControl
{
    public class MatchEnd : MonoBehaviour
    {
        private GameRoot gameRoot;
        [SerializeField] private TextMeshProUGUI matchEndText;
        private string textToType;
        public float typingSpeed = 0.1f;         // Time delay between each character being typed
        public float fadeDuration = 0.3f;          // Duration of the fade-out effect after typing is done

        private void Awake()
        {
            gameRoot = FindFirstObjectByType<GameRoot>();
        }
        private void OnEnable()
        {
            if (gameRoot == null)
            {
                Debug.LogError("MatchEnd: GameRoot component not found.");
                return;
            }
            gameRoot.gameEndedEvent.AddListener(OnMatchEnd);
        }

        private void OnMatchEnd(int winnerID)
        {
            gameRoot.countdownTimer.StopTimer();
            StartCoroutine(ShowEndMatchBanner(winnerID));
        }
    
        IEnumerator ShowEndMatchBanner(int winnerID)
        {
            gameRoot.countdownTimer.StopTimer();
            matchEndText.enabled = true;
            textToType = $"Player {winnerID}\nWins!";
            yield return StartCoroutine(TypeAndFadeText());
            yield return new WaitForSeconds(3f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneBuildIndex: 0);
        }
        private IEnumerator TypeAndFadeText()
        {
            matchEndText.text = "";  // Clear the text at the beginning

            // Type out each character
            foreach (char letter in textToType)
            {
                matchEndText.text += letter;  // Add one character at a time
                yield return new WaitForSeconds(typingSpeed);  // Wait before typing the next character
            }
            // After typing is done, start fading out the text
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(FadeOutText());
        }
        private IEnumerator FadeOutText()
        {
            float startAlpha = matchEndText.color.a;
            float endAlpha = 0f;
            float startTime = Time.time;

            // Fade out over 'fadeDuration' seconds
            while (Time.time - startTime < fadeDuration)
            {
                float lerpValue = (Time.time - startTime) / fadeDuration;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, lerpValue);
                matchEndText.color = new Color(matchEndText.color.r, matchEndText.color.g, matchEndText.color.b, alpha);
                yield return null;
            }

            // Ensure the text is fully transparent after fade-out
            matchEndText.color = new Color(matchEndText.color.r, matchEndText.color.g, matchEndText.color.b, endAlpha);
        }
    }
}
