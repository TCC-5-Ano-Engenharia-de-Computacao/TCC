using UnityEngine;
using UnityEngine.Serialization;

namespace Input
{
    public class InputRoot : MonoBehaviour
    {
        [FormerlySerializedAs("inputPerformedEventController")] public InputController inputController;
        public InputBuffer inputBuffer;
        public AnalogHistory analogHistory;
    }
}
