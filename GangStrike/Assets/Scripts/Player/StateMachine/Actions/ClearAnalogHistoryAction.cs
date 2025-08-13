using StateMachine.Attributes;

namespace StateMachine.Actions
{
    [XmlTag("ClearAnalogHistoryAction")]
    public sealed class ClearAnalogHistoryAction : ActionBase
    {
        public override void Execute(PlayerRoot owner)
        {
            owner.inputRoot.analogHistory.analogHistoryStr = "";
        }
    }
}