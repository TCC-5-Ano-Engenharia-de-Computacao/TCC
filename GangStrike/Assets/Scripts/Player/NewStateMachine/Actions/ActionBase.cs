// Player.NewStateMachine.Actions.ActionBase
using UnityEngine;
using StateMachine;

namespace Player.NewStateMachine.Actions
{
    public abstract class ActionBase : MonoBehaviour
    {
        // Execução sem parâmetros. Quem precisar de PlayerRoot guarda por conta própria.
        public abstract void Execute();
    }
}