using System.Collections.Generic;
using UnityEngine;

namespace Input
{
    public class AnalogHistory : MonoBehaviour
    {
        /*SerializedDebug*/public string analogHistoryStr = "";

        [SerializeField] private SideSwapper sideSwapper;
        [SerializeField] private int analogHistorySize = 50;
        [SerializeField] private InputController inputTest;
        [SerializeField] private float deadzone;
        

        private int _currentFrameDir;

    
    
        private enum AnalogDir
        {
            BackwardsDown = 1,
            Down, // = 2
            ForwardsDown, // = 3
            Backwards, // = 4
            Center, // = 5
            Forwards, // = 6
            BackwardsUp, // = 7
            Up, // = 8
            ForwardsUp, // = 9
        }

        private void Update()
        {
            var readValue = inputTest.playerInputActions.Default.AnalogStick.ReadValue<Vector2>();
            readValue.x *= sideSwapper.swapped ? -1 : 1;
            _currentFrameDir = (int) GetAnalogDir(readValue);
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

        private AnalogDir GetAnalogDir(Vector2 input)
        {
            // Deadzone
            if (input.sqrMagnitude < deadzone)
                return AnalogDir.Center;
            
            Vector2 normalized = input.normalized;
            
            Vector2[] dirs =
            {
                new Vector2(-1, -1).normalized, // SouthWest
                new Vector2(0, -1).normalized, // South
                new Vector2(1, -1).normalized, // SouthEast
                new Vector2(-1, 0).normalized, // West
                Vector2.zero, // Center (não usado aqui)
                new Vector2(1, 0).normalized, // East
                new Vector2(-1, 1).normalized, // NorthWest
                new Vector2(0, 1).normalized, // North
                new Vector2(1, 1).normalized, // NorthEast
            };
            
            float bestDot = -Mathf.Infinity;
            int bestIndex = 0;

            for (int i = 0; i < dirs.Length; i++)
            {
                if (dirs[i] == Vector2.zero) continue; // ignora o Center, já incluído na deadzone
                float dot = Vector2.Dot(normalized, dirs[i]);
                if (dot > bestDot)
                {
                    bestDot = dot;
                    bestIndex = i;
                }
            }

            return (AnalogDir)(bestIndex + 1);
        }
    }
}
