using UnityEngine;
using UnityEngine.Serialization;

namespace Input
{
    public class InputRoot : MonoBehaviour
    {
        public InputController inputController;
        public NewInputBuffer inputBuffer;
        public AnalogHistory analogHistory;
    }
}
