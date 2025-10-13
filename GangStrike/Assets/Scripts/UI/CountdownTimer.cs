using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace UI
{
    public class CountdownTimer : MonoBehaviour
    {
        [Header("Timer Settings")]
        [SerializeField] private float startTimeInSeconds = 120f;

        [FormerlySerializedAs("TimerEndedEvent")] [Header("Events")]
        public UnityEvent timerEndedEvent; // Event to notify when the timer ends
        [FormerlySerializedAs("TimerUpdatedEvent")] [FormerlySerializedAs("onTimerUpdated")] 
        public UnityEvent<float> timerUpdatedEvent; // Optional: Event to notify time updates
        private GameRoot gameRoot;
        
        private float remainingTime;
        private bool isRunning = false;

        public float RemainingTime => remainingTime;
        public bool IsRunning => isRunning;

        public void Awake()
        {
            gameRoot = FindFirstObjectByType<GameRoot>();
            gameRoot.RegisterCountdownTimer(this);
        }

        private void Start()
        {
            timerUpdatedEvent?.Invoke(startTimeInSeconds);
        }

        private void Update()
        {
            if (!isRunning) return;

            remainingTime -= Time.deltaTime;
            timerUpdatedEvent?.Invoke(remainingTime);

            if (remainingTime <= 0.1f)
            {
                remainingTime = 0f;
                isRunning = false;
                timerEndedEvent?.Invoke();
            }
        }

        /// <summary>
        /// Starts or restarts the timer.
        /// </summary>
        public void StartTimer()
        {
            remainingTime = startTimeInSeconds+1; // +1 to account for immediate decrement in Update
            isRunning = true;
        }

        /// <summary>
        /// Stops (pauses) the timer.
        /// </summary>
        public void StopTimer()
        {
            isRunning = false;
        }
        
        public void ResetTimer()
        {
            remainingTime = startTimeInSeconds;
            timerUpdatedEvent?.Invoke(remainingTime);
        }
        public bool IsTimerRunning()
        {
            return isRunning;
        }
    }
}