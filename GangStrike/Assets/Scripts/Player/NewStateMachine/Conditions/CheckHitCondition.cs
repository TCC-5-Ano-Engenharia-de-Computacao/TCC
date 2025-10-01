using Player.Input;

namespace Player.NewStateMachine.Conditions
{
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;

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
    public sealed class CheckHitCondition : ConditionBase
    {
        [SerializeField] private string hitEffect = "stagger";
        [SerializeField] private IncomingHitBuffer buffer;
        

        public override bool Evaluate()
        {
            return buffer.HasHitEffect(hitEffect);
        }
        
        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(CheckHitCondition));
            go.transform.SetParent(parent, false);

            var cond   = go.AddComponent<CheckHitCondition>();
            cond.buffer = player.characterRoot.incomingHitBuffer;

            var attr = (string)node.Attribute("hitEffect");
            if (!string.IsNullOrEmpty(attr)) cond.hitEffect = attr;

            return Task.FromResult<ConditionBase>(cond);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(CheckHitCondition), ConstructFromXmlAsync);
    }
}
