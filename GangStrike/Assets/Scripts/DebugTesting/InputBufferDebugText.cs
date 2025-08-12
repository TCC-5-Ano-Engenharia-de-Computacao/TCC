using Input;
using StateMachine;
using TMPro;
using UnityEngine;

public class InputBufferDebugText : MonoBehaviour
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

    private void Update()
    {
        if (playerRoot != null && playerRoot.inputRoot.inputBuffer != null)
        {
            var inputBuffer = playerRoot.inputRoot.inputBuffer;
            if (inputBuffer != null)
            {
                string debugInfo = inputBuffer.GetFormattedInputBuffer();
                _textMeshPro.text = debugInfo;
            }
        }
        Debug.Log("No Reference to PlayerRoot or InputBuffer");
    }
}