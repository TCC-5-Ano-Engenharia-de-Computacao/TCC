using System.Collections.Generic;
using UnityEngine;

namespace Examples
{
    // Helper component for hits
    public class PlayerHitbox : MonoBehaviour
    {
        private readonly Queue<string> _hits = new();
        public void RegisterHit(string hitType) => _hits.Enqueue(hitType);
        public bool TryConsumeHit(out string type)
        {
            if (_hits.Count > 0)
            {
                type = _hits.Dequeue();
                return true;
            }
            type = null;
            return false;
        }
    }
}