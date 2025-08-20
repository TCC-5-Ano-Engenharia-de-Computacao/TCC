// Player.NewStateMachine.Actions.ClearBufferedInputsAction.cs
namespace Player.NewStateMachine.Actions
{
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;
    using Input;   // NewInputBuffer

    /// <summary>
    /// Remove todos os comandos armazenados no <see cref="NewInputBuffer"/>.
    /// </summary>
    public sealed class ClearBufferedInputsAction : ActionBase
    {
        private NewInputBuffer _buffer;

        public override void Execute()
        {
            _buffer.Clear();
        }

        // ------------------------------ XML Factory -------------------------
        public static Task<ActionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(ClearBufferedInputsAction));
            go.transform.SetParent(parent, false);

            var action  = go.AddComponent<ClearBufferedInputsAction>();
            action._buffer = player.inputRoot.inputBuffer as NewInputBuffer;

            return Task.FromResult<ActionBase>(action);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ActionFactory.Register(nameof(ClearBufferedInputsAction), ConstructFromXmlAsync);
    }
}