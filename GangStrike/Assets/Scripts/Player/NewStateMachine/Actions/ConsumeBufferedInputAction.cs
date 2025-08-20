// Player.NewStateMachine.Actions.ConsumeBufferedInputAction.cs
namespace Player.NewStateMachine.Actions
{
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;
    using Input; // NewInputBuffer

    /// <summary>
    /// Marca um comando específico como “consumido” no <see cref="NewInputBuffer"/>.
    /// Basta consultá-lo; o próprio buffer remove o comando no <c>EndFrame</c>.
    /// </summary>
    public sealed class ConsumeBufferedInputAction : ActionBase
    {
        [SerializeField] private string actionName = "Jump";

        private NewInputBuffer _buffer;

        public override void Execute()
        {
            // Basta qualquer consulta para marcar triedConsume = true
            _ = _buffer.StartedThisFrame(actionName);
            _ = _buffer.IsLingering(actionName);
        }

        // ------------------------------ XML Factory -------------------------
        public static Task<ActionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(ConsumeBufferedInputAction));
            go.transform.SetParent(parent, false);

            var action   = go.AddComponent<ConsumeBufferedInputAction>();
            action._buffer = player.inputRoot.inputBuffer as NewInputBuffer;

            // <ConsumeBufferedInputAction inputName="Punch"/>
            var attr = (string?)node.Attribute("inputName");
            if (!string.IsNullOrEmpty(attr))
                action.actionName = attr;

            return Task.FromResult<ActionBase>(action);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ActionFactory.Register(nameof(ConsumeBufferedInputAction), ConstructFromXmlAsync);
    }
}