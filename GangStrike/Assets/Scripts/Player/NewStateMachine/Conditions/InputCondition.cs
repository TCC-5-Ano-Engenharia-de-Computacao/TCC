// Player.NewStateMachine.Conditions.BufferedInputCondition.cs
namespace Player.NewStateMachine.Conditions
{
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;
    using Input;   // NewInputBuffer vive aqui

    /// <summary>
    /// Condição que verifica se um comando (ex.: “Jump”, “Punch”)
    /// foi disparado no <see cref="NewInputBuffer"/>.
    /// 
    /// ✔  <b>requireInstantPress</b> — valida apenas se o botão foi
    ///    pressionado <i>neste</i> frame.
    /// ✔  Caso falso, também considera o estado de lingering.
    /// ✔  <b>consumeOnMatch</b> — retira o input quando a condição
    ///    retornar <c>true</c>; útil para não disparar de novo.
    /// </summary>
    public sealed class BufferedInputCondition : ConditionBase
    {
        // ------------------------------- Inspector
        [SerializeField] private string actionName = "Jump";

        [Tooltip("Apenas no frame em que o botão foi pressionado.")]
        [SerializeField] private bool requireInstantPress = false;

        [Tooltip("Remove o input ao final do mesmo frame em que casar.")]
        [SerializeField] private bool consumeOnMatch = false;

        // ------------------------------- Runtime
        private NewInputBuffer _buffer;

        public override bool Evaluate()
        {
            // Lógica de validação
            bool pressedNow    = _buffer.StartedThisFrame(actionName);
            bool stillBuffered = _buffer.IsLingering(actionName);

            bool matched = requireInstantPress
                           ? pressedNow
                           : (pressedNow || stillBuffered);

            // Se não quisermos consumir, precisamos “recolocar” o input
            // (os métodos acima marcam triedConsume=true).
            if (matched && !consumeOnMatch)
                _buffer.RegisterInput(actionName);

            return matched;
        }

        // -------------------------- Fábrica / XML ---------------
        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(BufferedInputCondition));
            go.transform.SetParent(parent, false);

            var cond   = go.AddComponent<BufferedInputCondition>();
            cond._buffer = player.inputRoot.inputBuffer;

            string? attr;

            attr = (string?)node.Attribute("inputName");
            if (!string.IsNullOrEmpty(attr)) cond.actionName = attr;

            attr = (string?)node.Attribute("isInstantaneous");
            if (!string.IsNullOrEmpty(attr) && bool.TryParse(attr, out var inst))
                cond.requireInstantPress = inst;

            attr = (string?)node.Attribute("removeInputOnEndOfFrame");
            if (!string.IsNullOrEmpty(attr) && bool.TryParse(attr, out var rem))
                cond.consumeOnMatch = rem;

            return Task.FromResult<ConditionBase>(cond);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(BufferedInputCondition), ConstructFromXmlAsync);
    }
}
