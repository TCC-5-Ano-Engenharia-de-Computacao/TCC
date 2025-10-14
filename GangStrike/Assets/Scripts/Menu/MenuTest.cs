using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Menu
{
    public class MenuTest : MonoBehaviour
    {
        [SerializeField]private Button startButton;
        private GameRoot gameRoot;
        private void Start()
        {
            gameRoot = FindFirstObjectByType<GameRoot>();
            if (gameRoot == null)
            {
                Debug.LogError("MenuTest: GameRoot not found in the scene!");
                return;
            }
            gameRoot.ResetEverything();
        }
        
        public void OnStartButtonPressed()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
        
        public void OnQuitButtonPressed()
        {
            Application.Quit();
            
        }
    }
}
