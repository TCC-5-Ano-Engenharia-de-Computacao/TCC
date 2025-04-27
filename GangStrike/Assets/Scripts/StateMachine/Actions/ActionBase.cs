using UnityEngine;

namespace StateMachine.Actions
{
    /// <summary>
    /// Base de todas as ações.  Cada Action executa lógica de jogo quando o estado apropriado dispara.
    /// </summary>
    public abstract class ActionBase
    {
        public virtual void Initialize(GameObject owner) { }
        public abstract void Execute(GameObject owner);
    }
}