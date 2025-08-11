// Player.NewStateMachine.Conditions.ConditionBase
using UnityEngine;
using StateMachine;

namespace Player.NewStateMachine.Conditions
{
    public abstract class ConditionBase : MonoBehaviour
    {
        public abstract bool Evaluate();
    }
}