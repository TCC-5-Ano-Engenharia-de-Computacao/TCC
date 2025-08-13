using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Input;
using StateMachine.Attributes;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace StateMachine.Conditions
{
    /// <summary>
    /// Sempre retorna verdadeiro.
    /// </summary>
    [XmlTag("PlayerTimerCondition")]
    public sealed class PlayerTimerCondition : ConditionBase
    {
        [XmlAttribute("timerName")] public string timerName { get; set; } = "TestTimer";
        private PlayerSimpleTimer _playerSimpleTimer;
        public override Task Initialize(PlayerRoot owner)
        {
            _playerSimpleTimer = owner.PlayerSimpleTimer;
            return Task.CompletedTask;
        }

        public override bool Evaluate(PlayerRoot owner)
        {
            return _playerSimpleTimer.IsTimerComplete(timerName);
        }
    }
}