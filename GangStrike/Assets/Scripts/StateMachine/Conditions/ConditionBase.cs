using UnityEngine;

namespace StateMachine.Conditions
{
    public abstract class ConditionBase
    {
        public virtual void Initialize(PlayerRoot owner) { }
        public abstract bool Evaluate(PlayerRoot owner);

        public virtual string ToDebugString(int indentationLevel = 0)
        {
            var indentation = new string(' ', indentationLevel * 2);
            return indentation + GetType().Name;
        }
    }
}