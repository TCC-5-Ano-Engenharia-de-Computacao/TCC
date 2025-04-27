using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnalogHistory : MonoBehaviour
{
    /*SerializedDebug*/public string analogHistoryStr = "";
    
    [SerializeField] private int analogHistorySize = 50;
    [SerializeField] private InputPerformedEventController inputTest;

    private int _currentFrameDir;

    
    private enum AnalogDir
    {
        SouthWest = 1,
        South, // = 2
        SouthEast, // = 3
        West, // = 4
        Center, // = 5
        East, // = 6
        NorthWest, // = 7
        North, // = 8
        NorthEast, // = 9
    }

    private readonly Dictionary<Vector2, AnalogDir> _eightDirMap = new()
    {
        {new Vector2(-1, -1).normalized, AnalogDir.SouthWest},
        {Vector2.down, AnalogDir.South},
        {new Vector2(1, -1).normalized, AnalogDir.SouthEast},
        {Vector2.left, AnalogDir.West},
        {Vector2.zero, AnalogDir.Center},
        {Vector2.right, AnalogDir.East},
        {new Vector2(-1, 1).normalized, AnalogDir.NorthWest},
        {Vector2.up, AnalogDir.North},
        {new Vector2(1, 1).normalized, AnalogDir.NorthEast},
    };

    private void Update()
    {
        _currentFrameDir = (int) _eightDirMap[inputTest.playerInputActions.Default.AnalogStick.ReadValue<Vector2>().normalized];
        AddToDirRegex(_currentFrameDir);
    }

    private void AddToDirRegex(int dir)
    {
        if (analogHistoryStr.Length < analogHistorySize)
        {
            analogHistoryStr += dir;
        }
        else
        {
            analogHistoryStr = analogHistoryStr.Remove(0, 1) + dir;
        }
    }
}
