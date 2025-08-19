// Player.NewStateMachine.Conditions.AnalogHistoryRegexCondition.cs
namespace Player.NewStateMachine.Conditions
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using StateMachine;
    using UnityEngine;
    using Input;

    public sealed class AnalogHistoryRegexCondition : ConditionBase
    {
        [SerializeField] private string regexString;
        private AnalogHistory _analogHistory;

        public override bool Evaluate()
        {
            var historyString = _analogHistory.analogHistoryStr;
            return Regex.IsMatch(historyString, regexString);
        }

        public static Task<ConditionBase> ConstructFromXmlAsync(
            XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(AnalogHistoryRegexCondition));
            go.transform.SetParent(parent, false);

            var c = go.AddComponent<AnalogHistoryRegexCondition>();
            c._analogHistory = player.inputRoot.analogHistory;

            var attr = (string)node.Attribute("regexString");
            if (!string.IsNullOrEmpty(attr))
            {
                c.regexString = attr;
            }
            else
            {
                Debug.LogError($"{nameof(AnalogHistoryRegexCondition)}: atributo 'regexString' ausente ou vazio.");
            }

            return Task.FromResult<ConditionBase>(c);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ConditionFactory.Register(nameof(AnalogHistoryRegexCondition), ConstructFromXmlAsync);
    }
}