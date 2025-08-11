using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Input;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Conditions
{
    [XmlTag("AnalogHistoryRegexCondition")]
    public sealed class AnalogHistoryRegexCondition : ConditionBase
    {
        [XmlAttribute("regexString")] public string RegexString { get; set; }
        private AnalogHistory _analogHistory;
        
        public override async Task Initialize(PlayerRoot owner)
        {
            if (RegexString == null)
            {
                Debug.LogError("Condicao regex com string nula: ");
            }
            _analogHistory = owner.inputRoot.analogHistory;
        }
        public override bool Evaluate(PlayerRoot owner)
        {
            var historyString = _analogHistory.analogHistoryStr;
            return Regex.Match(historyString, RegexString).Success;
        }
    }
}