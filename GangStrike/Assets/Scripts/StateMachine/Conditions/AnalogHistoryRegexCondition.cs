using System.Text.RegularExpressions;
using Input;

namespace StateMachine.Conditions
{
    //todo: adicionar no xml
    public sealed class AnalogHistoryRegexCondition : ConditionBase
    {
        //todo: fazer funcionar de um jeito melhor e ajeitar o analog history
        public string regexstring { get; set; } = "111";
        public bool clearHistoryOnSuccess { get; set; } = false;
        
        private AnalogHistory _analogHistory;
        public void TransitionResult(bool sucess)
        {
            if (clearHistoryOnSuccess)
            {
                if (sucess)
                {
                    _analogHistory.analogHistoryStr.Remove(0,_analogHistory.analogHistoryStr.Length);
                }
            }
            //todo : faz essa função ser chamada pela maquina de estado
        }
        public override void Initialize(PlayerRoot owner)
        {
            _analogHistory = owner.GetComponent<AnalogHistory>();
        }
        public override bool Evaluate(PlayerRoot owner)
        {
            
            var history = _analogHistory.analogHistoryStr;

            var match = Regex.Match(history, regexstring);
            if (match.Success)
            {
                return true;
            }
            return false;
        }
    }
}