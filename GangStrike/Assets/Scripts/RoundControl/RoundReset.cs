using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoundControl
{
    public class RoundReset : MonoBehaviour
    {
        private RoundController roundController;

        private void Awake()
        {
            roundController = GetComponent<RoundController>();
        }
        
        private void OnEnable()
        {
            if (roundController == null)
            {
                Debug.LogError("RoundReset: RoundController component not found.");
                return;
            }
            roundController.roundResetEvent.AddListener(ReloadScene);
        }
        private void OnDisable()
        {
            if (roundController == null)
            {
                Debug.LogError("RoundReset: RoundController component not found.");
                return;
            }
            roundController.roundResetEvent.RemoveListener(ReloadScene);
        }

        private void ReloadScene()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
        public void ForceReload()
        {
            ReloadScene();
        }
    }
}
