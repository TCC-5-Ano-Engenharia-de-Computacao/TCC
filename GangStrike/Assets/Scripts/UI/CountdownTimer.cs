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

        private float remainingTime;
        private bool isRunning = false;

        public float RemainingTime => remainingTime;
        public bool IsRunning => isRunning;

        public void Start()
        {
            // Initialize and start the timer for testing, later can be controlled via methods from GameController
            StartTimer();
        }

        private void Update()
        {
            if (!isRunning) return;

            remainingTime -= Time.deltaTime;
            timerUpdatedEvent?.Invoke(remainingTime);

            if (remainingTime <= 0f)
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
            remainingTime = startTimeInSeconds;
            isRunning = true;
        }

        /// <summary>
        /// Stops (pauses) the timer.
        /// </summary>
        public void StopTimer()
        {
            isRunning = false;
        }
    }
}