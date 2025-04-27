using UnityEngine;

namespace StateMachine.Conditions
{
    public abstract class ConditionBase
    {
        public virtual void Initialize(GameObject owner) { }
        public abstract bool Evaluate(GameObject owner);
    }
}