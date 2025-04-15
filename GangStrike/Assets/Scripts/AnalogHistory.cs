using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnalogHistory : MonoBehaviour
{
    /*SerializedDebug*/public string analogHistoryStr = "";
    
    [SerializeField] private readonly int analogHistorySize = 30;
    [SerializeField] private InputTest inputTest;

    
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
    
    private void OnEnable()
    {
        inputTest.playerInputActions.Default.AnalogStick.performed += HandleAnalogPerformed;
        inputTest.playerInputActions.Default.AnalogStick.canceled += HandleAnalogCanceled;
    }

    private void OnDisable()
    {
        inputTest.playerInputActions.Default.AnalogStick.performed -= HandleAnalogPerformed;
        inputTest.playerInputActions.Default.AnalogStick.canceled -= HandleAnalogCanceled;
    }

    private void HandleAnalogPerformed(InputAction.CallbackContext ctx)
    {
        AnalogDir dir = _eightDirMap[ctx.ReadValue<Vector2>().normalized]; 
        //Debug.Log(dir);
    }
    
    private void HandleAnalogCanceled(InputAction.CallbackContext ctx)
    {
        AnalogDir dir = _eightDirMap[ctx.ReadValue<Vector2>().normalized]; 
        //Debug.Log(dir);
    }

    private void AddToDirRegex(int dir)
    {
        // TODO{discutir} apenas comecar a string preenchida com varios 5 (Center / neutro)
        if (analogHistoryStr.Length < analogHistorySize)
        {
            analogHistoryStr += dir;
        }
        else
        {
            analogHistoryStr = analogHistoryStr.Remove(0) + dir;
        }
        Debug.Log(analogHistoryStr);
    }
}
