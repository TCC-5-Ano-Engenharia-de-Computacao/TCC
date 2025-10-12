using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RoundControl
{
    public class RoundController : MonoBehaviour
    {
        private RoundStart roundStart;
        private RoundEnd roundEnd;
        private RoundReset roundReset;
        public UnityEvent roundResetEvent;
        
        private void Awake()
        {
            roundStart = GetComponent<RoundStart>();
            roundEnd = GetComponent<RoundEnd>();
            roundReset = GetComponent<RoundReset>();
        }
    }
}
