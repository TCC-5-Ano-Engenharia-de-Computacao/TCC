namespace StateMachine.Conditions
{
    //todo: adicionar no xml

    public sealed class InputCondition : ConditionBase
    {        //todo: fazer funcionar de um jeito melhor e ajeitar o InputBuffer

        public string InputName { get; set; } = "Jump";
        public bool isIstantanius { get; set; } = false;
        public bool removeInputOnEndOFFrame { get; set; } = false;
        public bool removeInstantaniusOnTransitionSuccess { get; set; } = false;
        
        private InputBuffer _inputBuffer;

        public void TransitionResult(bool sucess)
        {
            if (removeInstantaniusOnTransitionSuccess)
            {
                if (sucess)
                {
                    _inputBuffer.ConsumeInput(InputName);
                }
            }
            //todo : faz essa função ser chamada pela maquina de estado
        }
        public override void Initialize(RootCharacter owner)
        {
            _inputBuffer = owner.GetComponent<InputBuffer>();
        }
        public override bool Evaluate(RootCharacter owner)
        {
            if (isIstantanius)
            {
                
            }
            else
            {
                var inQueue = _inputBuffer.IsInputInQueue(InputName);
                var inInstantaneous = _inputBuffer.IsInputInstantaneous(InputName);
                
                
                return inQueue || inInstantaneous;
            }

            return false;
        }
    }
}