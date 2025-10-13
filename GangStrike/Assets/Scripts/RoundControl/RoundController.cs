using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RoundControl
{
    public class RoundController : MonoBehaviour
    {
        public RoundStart roundStart;
        public RoundEnd roundEnd;
        public RoundReset roundReset;
        public UnityEvent roundResetEvent;
        private GameRoot gameRoot;
        
        private void Awake()
        {
            gameRoot = FindFirstObjectByType<GameRoot>();
            roundStart = GetComponent<RoundStart>();
            roundEnd = GetComponent<RoundEnd>();
            roundReset = GetComponent<RoundReset>();
            gameRoot.RegisterRoundController(this);
        }
    }
}
