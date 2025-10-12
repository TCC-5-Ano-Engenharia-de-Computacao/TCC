using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class MatchEnd : MonoBehaviour
{
    private GameRoot gameRoot;

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
        // Get the center of the screen in world coordinates
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);

        // Convert the screen center to world coordinates
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

        // Instantiate the prefab at the world center position
        // Uncomment the following line and replace 'prefab' with your actual prefab variable
        // Instantiate(prefab, worldCenter, Quaternion.identity);

        // Wait for 3 seconds, then return to menu
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}
