using Input;
using UnityEngine;

namespace StateMachine
{
    public class PlayerRoot : MonoBehaviour
    {
        public CharacterRoot characterRoot;
        public StateMachine stateMachine;
        public InputRoot inputRoot;
        public PlayerSimpleTimer PlayerSimpleTimer;
        //TEMPORARY, FOR TESTING
        public StateTimerDebugText stateTimerDebugText;
    }
}