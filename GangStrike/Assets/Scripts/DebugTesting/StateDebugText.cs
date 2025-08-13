/*
using StateMachine;
using TMPro;
using UnityEngine;

public class StateDebugText : MonoBehaviour
{
    public PlayerRoot playerRoot;
    private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        if (_textMeshPro == null)
        {
            Debug.LogError("TextMeshPro component not found on this GameObject.");
            return;
        }
    }

    public void Update()
    {
        if (playerRoot != null && playerRoot.stateMachine != null)
        {
            var stateMachine = playerRoot.stateMachine;
            if (stateMachine != null)
            {
                string debugInfo = stateMachine.GetCurrentStateDebugInfo();
                _textMeshPro.text = debugInfo;
            }
        }
        else
        {
            Debug.Log("No Reference to PlayerRoot or StateMachine");
        }
    }
}
*/
