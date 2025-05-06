using UnityEngine;

namespace StateMachine.Actions
{
    /// <summary>
    /// Base de todas as ações.  Cada Action executa lógica de jogo quando o estado apropriado dispara.
    /// </summary>
    public abstract class ActionBase
    {
        public virtual void Initialize(RootCharacter owner) { }
        public abstract void Execute(RootCharacter owner);

        public virtual string ToDebugString(int indentationLevel = 0)
        {
            var indentation = new string(' ', indentationLevel * 2);
            return indentation + GetType().Name;
        }
    }
}