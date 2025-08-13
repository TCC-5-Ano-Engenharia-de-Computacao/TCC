using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Actions
{
    [XmlTag("SetPlayerTimerAction")]
    public class SetPlayerTimerAction : ActionBase
    {
        [XmlAttribute("timerName")] public string TimerName { get; set; } = "DefaultTimer";
        [XmlAttribute("timerValue")] public float TimerValue { get; set; } = 0.0f;

        public override void Execute(PlayerRoot owner)
        {
            owner.PlayerSimpleTimer.SetPlayerRoot(owner);
            owner.PlayerSimpleTimer.SetTimer(TimerName, TimerValue);
        }

        public override string ToDebugString(int indentationLevel = 0)
        {
            var indentation = new string(' ', indentationLevel * 2);
            return $"{indentation}{GetType().Name} (TimerName: {TimerName}, TimerValue: {TimerValue})";
        }
    }
}