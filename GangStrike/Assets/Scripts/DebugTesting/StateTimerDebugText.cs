using TMPro;
using UnityEngine;

public class StateTimerDebugText : MonoBehaviour
{
    public PlayerSimpleTimer playerSimpleTimer;
    private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        if (_textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject.");
        }
    }
    
    public void SetText(string text)
    {
        if (_textMeshPro != null)
        {
            _textMeshPro.text = text;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is not assigned.");
        }
    }
}