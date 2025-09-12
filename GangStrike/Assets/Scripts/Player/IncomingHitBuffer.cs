using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class IncomingHitBuffer : MonoBehaviour
    {
        public List<Hit> hitBuffer = new List<Hit>();
        
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
