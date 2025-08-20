/*
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Input;
using StateMachine.Attributes;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace StateMachine.Conditions
{
    [XmlTag("InputCondition")]

    public sealed class InputCondition : ConditionBase
    {        //todo: fazer funcionar de um jeito melhor e ajeitar o InputBuffer
        [XmlAttribute("inputName")] public string inputName { get; set; } = "Jump";
        [XmlAttribute("isInstantaneous")] public string isInstantaneous { get; set; } = "false";
        [XmlAttribute("removeInputOnEndOfFrame")] public string removeInputOnEndOfFrame { get; set; } = "false";
       
        private InputBuffer _inputBuffer;
        
        public override Task Initialize(PlayerRoot owner)
        {
            _inputBuffer = owner.inputRoot.inputBuffer;
            return Task.CompletedTask;
        }
        public override bool Evaluate(PlayerRoot owner)
        {
            var inQueue = _inputBuffer.IsInputInQueue(inputName);
            var inInstantaneous = _inputBuffer.IsInputInstantaneous(inputName);
            if(isInstantaneous == "true")
            {
                if (inInstantaneous)
                {
                    if (removeInputOnEndOfFrame == "true")
                    {
                        _inputBuffer.ConsumeInput(inputName);
                    }
                    return true;
                }
            }
            else
            {
                // if inQueue OR inInstantaneous
                if (inQueue || inInstantaneous)
                {
                    if (removeInputOnEndOfFrame == "true")
                    {
                        _inputBuffer.ConsumeInput(inputName);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}*/