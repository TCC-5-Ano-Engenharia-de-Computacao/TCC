using StateMachine;
using UnityEngine;

namespace UI
{
    public class CameraMover : MonoBehaviour
    {
        [Header("Follow Settings")]
        public float followSpeed = 4f;

        [Header("X Movement Limits")]
        public float minX = -100f;
        public float maxX = 100f;

        public Transform player1;
        public Transform player2;

        void Start()
        {
            InvokeRepeating(nameof(FindPlayers), 0f, 0.5f); // Look for players every 0.5 seconds
        }

        void FindPlayers()
        {
            PlayerRoot[] players = FindObjectsOfType<PlayerRoot>();

            if (players.Length >= 2)
            {
                player1 = players[0].characterRoot.transform;
                player2 = players[1].characterRoot.transform;
                CancelInvoke(nameof(FindPlayers)); // Stop checking
            }
        }

        void LateUpdate()
        {
            if (player1 != null && player2 != null)
            {
                float meanX = (player1.position.x + player2.position.x) / 2f;

                // Clamp the target X within the limits
                float clampedX = Mathf.Clamp(meanX, minX, maxX);

                Vector3 targetPos = new Vector3(clampedX, transform.position.y, transform.position.z);

                transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
            }
        }
    }
}