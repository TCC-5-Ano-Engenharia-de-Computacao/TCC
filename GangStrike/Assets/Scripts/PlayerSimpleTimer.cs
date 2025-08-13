using System.Collections.Generic;
using System.Linq;
using StateMachine;
using UnityEngine;

public class PlayerSimpleTimer : MonoBehaviour
{
    private PlayerRoot _playerRoot;
    private readonly Dictionary<string, float> _timers = new Dictionary<string, float>();

    public void SetPlayerRoot(PlayerRoot playerRoot)
    {
        _playerRoot = playerRoot;
    }
    
    public float GetTimer(string timerName)
    {
        return _timers.ContainsKey(timerName) ? _timers[timerName] : 0f;
    }
    
    public void SetTimer(string timerName, float value)
    {
        if (!_timers.ContainsKey(timerName))
        {
            _timers.Add(timerName, value);
            Debug.Log($"Adding new timer: {timerName}");
        }
        else
        {
            _timers[timerName] = Mathf.Max(0, value);
        }
    }

    private void Update()
    {
        var keys = new List<string>(_timers.Keys);
        foreach (var timerName in keys)
        {
            // Logs the list of timers;
            if (_timers[timerName] > 0)
            {
                _timers[timerName] -= Time.deltaTime; // Decrease the timer over time
            }
        }
        _playerRoot.stateTimerDebugText.SetText(GetAllTimersAsString());
    }

    public bool IsTimerComplete(string timerName)
    {
        return _timers[timerName] <= 0;
    }
    
    private string GetAllTimersAsString()
    {
        if (_timers.Count == 0)
        {
            return "No active timers.";
        }

        return string.Join("\n", _timers.Select(kvp => $"{kvp.Key}: {kvp.Value:F2}s"));
    }
}