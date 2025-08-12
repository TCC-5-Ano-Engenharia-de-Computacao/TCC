using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StateMachine.Actions;
using StateMachine.Conditions;
using StateMachine.Model;
using StateMachine.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace StateMachine
{
    public class StateMachine:MonoBehaviour
    {
        private StateMachineModel _stateMachineModel;
        private StateModel _currentStateModel;
        [SerializeField] private PlayerRoot playerRoot;
        
        [SerializeField] private string filePath = "Assets/Scripts/Examples/state_machine.xml";

        private void Start()
        {
            var serializer = StateMachineSerializerFactory.Get();
            using var fs = File.OpenRead(filePath);
            _stateMachineModel = (StateMachineModel)serializer.Deserialize(fs);
            _stateMachineModel.Initialize(playerRoot);
            
            SetStateById(_stateMachineModel.InitialState);
            
            Debug.Log($"StateMachineModel :\n{_stateMachineModel.ToDebugString(1)}");
            
            DebugPrint();
            //PrintStateMachine();
        }

        private void Update()
        {
            if (_currentStateModel != null)
            {
                _currentStateModel.DoStay(playerRoot);
                
                var validTransitionModel = _currentStateModel.EvaluateTransitions(playerRoot);
                
                if (validTransitionModel != null)
                {
                    SetStateById(validTransitionModel.To);
                }
            }
        }

        

        private void SetStateById(string id,List<StateModel> history = null)
        {
            if (_stateMachineModel != null)
            {
                var state = _stateMachineModel.GetStateFromId(id);
                if (state != null)
                {
                    SetStateByModel(state,history);
                }
                else
                {
                    Debug.LogError($"State with id '{id}' not found in the state machine.");
                }
            }
            else
            {
                Debug.LogError("State machine model is not initialized.");
            }
        }
        private void SetStateByModel(StateModel state,List<StateModel> history = null)
        {
            history ??= new List<StateModel>();
            if (history.Contains(state))
            {
                Debug.LogError($"State '{state.Id}' is already in the history. Avoiding infinite loop. ({history})");
                return;
            }
            history.Add(state);
            
            
            _currentStateModel?.DoLeave(playerRoot);

            _currentStateModel = state;

            if (_currentStateModel!=null)
            {
                _currentStateModel.DoBeforeEnter(playerRoot);

                var validTransitionModel = _currentStateModel.EvaluateTransitions(playerRoot); 
                
                if (validTransitionModel != null)
                {
                    SetStateById(validTransitionModel.To,history);
                }
                else
                {
                    _currentStateModel.DoEnter(playerRoot);
                }
            }
                
            
        }

        private void DebugPrint()
        {
            Debug.Log("StateMachine{ \n" +
                      (_currentStateModel != null ? $"Current State: {_currentStateModel.Id}" : "No current state") +
                      "\n}");
        }

        public string GetCurrentStateDebugInfo()
        {
            if (_currentStateModel == null)
            {
                return "Current State: None";
            }

            return $"Current State: {_currentStateModel.Id}\n" +
                   $"Transitions: {string.Join(", ", _currentStateModel.Transitions.Select(t => t.To))}";
        }
    }
}