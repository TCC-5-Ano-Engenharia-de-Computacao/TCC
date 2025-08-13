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
/*
namespace StateMachine
{
    public class StateMachine:MonoBehaviour
    {
        
        [SerializeField] private TextAsset stateMachineXMLAsset;
        
        private StateMachineDefinition _stateMachineDefinition;
        private StateMachineInstance _stateMachineInstance;
        
        private StateInstance _currentStateModel;
        private PlayerRoot _playerRoot;

        private void Awake()
        {
            _playerRoot = GetComponentInParent<PlayerRoot>();
        }
        
        
    
        private async void Initialize()
        {
            try
            {
                var serializer = StateMachineSerializerSingleton.Get();

                using (var reader = new StringReader(stateMachineXMLAsset.text))
                {
                    _stateMachineDefinition = (StateMachineDefinition)serializer.Deserialize(reader);
                }

                await _stateMachineDefinition.Initialize(_playerRoot);
                SetStateById(_stateMachineDefinition.InitialState);
            }
            catch (InvalidOperationException ex)
            {
                // Erros específicos de XmlSerializer (XML malformado ou tipo incompatível)
                Debug.LogError($"Erro ao desserializar máquina de estados: {ex.Message}\n{ex.InnerException}");
            }
            catch (Exception ex)
            {
                // Qualquer outro erro inesperado
                Debug.LogError($"Falha ao inicializar StateMachine: {ex}");
            }
        }


        private void Update()
        {
            if (_currentStateModel != null)
            {
                _currentStateModel.DoStay(_playerRoot);
                
                var validTransitionModel = _currentStateModel.EvaluateTransitions(_playerRoot);
                
                if (validTransitionModel != null)
                {
                    SetStateById(validTransitionModel.To);
                }
            }
        }

        

        private void SetStateById(string id,List<StateModel> history = null)
        {
            if (_stateMachineDefinition != null)
            {
                var state = _stateMachineDefinition.GetStateFromId(id);
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
            
            
            _currentStateModel?.DoLeave(_playerRoot);

            _currentStateModel = state;

            if (_currentStateModel!=null)
            {
                _currentStateModel.DoBeforeEnter(_playerRoot);

                var validTransitionModel = _currentStateModel.EvaluateTransitions(_playerRoot); 
                
                if (validTransitionModel != null)
                {
                    SetStateById(validTransitionModel.To,history);
                }
                else
                {
                    _currentStateModel.DoEnter(_playerRoot);
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

*/