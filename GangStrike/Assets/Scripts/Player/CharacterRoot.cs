using Player;
using UnityEngine;

namespace StateMachine
{
    public class CharacterRoot : MonoBehaviour
    {
        public AudioSource audioSource;
        public Animator animator;
        public Rigidbody2D rigidbody2D;
        public SideSwapper sideSwapper;
        public IncomingHitBuffer incomingHitBuffer;
        public Collider2D footGroundCollider;
        public Collider2D bodyCollider;
        public AttackTriggers attackTriggers;

    }
}