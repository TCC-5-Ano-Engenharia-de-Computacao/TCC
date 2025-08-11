using UnityEngine;
using UnityEngine.Serialization;

namespace Input
{
    public class InputRoot : MonoBehaviour
    {
        public InputController inputController;
        public InputBuffer inputBuffer;
        public AnalogHistory analogHistory;
    }
}
