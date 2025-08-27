using UnityEngine;

namespace StateMachine
{
    public class CharacterRoot : MonoBehaviour
    {
        public AudioSource audioSource;
        public Animator animator;
        public Rigidbody2D rigidbody2D;
        public Collider2D footGroundCollider;
        public SideSwapper sideSwapper;

    }
}