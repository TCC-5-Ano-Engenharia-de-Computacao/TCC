using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class IncomingHitBuffer : MonoBehaviour
    {
        private readonly List<Hit> hitBufferList = new();
        [SerializeField] private PlayerRoot playerRoot;
        
        public bool isInvulnerable = false;
        

        public void AddHitToBuffer(Hit hit)
        {
            if (!isInvulnerable)
            {
                hitBufferList.Add(hit);
                ApplyDamage(hit.damage);
            }
        }
        
        public bool HasHitEffect(string hitEffect)
        {
            return hitBufferList.Find(hit => hit.effect == hitEffect) != null;
        }

        public void ClearHitBuffer()
        {
            hitBufferList.Clear();
        }

        private void ApplyDamage(float damage)
        {
            playerRoot.attributeSystem.TakeDamage(damage);
        }
        
        public class Hit // Trocar por struct?
        {
            public float damage;
            public string effect; // Trocar por Enum ?
            public string tag;
            public float sameAttackCooldown;
            public float genericCooldown;
            public float knockBackForce;
            public float stunDuration;

            public Hit(float damage, string effect, string tag, float sameAttackCooldown=0f, float genericCooldown=0f, float knockBackForce=0f, float stunDuration=0f)
            {
                this.damage = damage;
                this.effect = effect;
                this.tag = tag;
                this.sameAttackCooldown = sameAttackCooldown;
                this.genericCooldown = genericCooldown;
                this.knockBackForce = knockBackForce;
                this.stunDuration = stunDuration;
            }
        }
    }
}
