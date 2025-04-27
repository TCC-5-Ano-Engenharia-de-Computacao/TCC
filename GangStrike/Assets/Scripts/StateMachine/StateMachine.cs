using System;
using System.Collections.Generic;
using System.IO;
using StateMachine.Actions;
using StateMachine.Conditions;
using StateMachine.Model;
using StateMachine.Serialization;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine:MonoBehaviour
    {
        private StateMachineModel _stateMachineModel;
        private StateModel _currentStateModel;
        public string filePath = "Assets/Scripts/Examples/state_machine.xml";

        private void Start()
        {
            var serializer = StateMachineSerializerFactory.Get();
            using var fs = File.OpenRead(filePath);
            _stateMachineModel = (StateMachineModel)serializer.Deserialize(fs);
            
            // TODO: Invocar metodos Initialize das Actions e Conditions
            // TODO: Inicializar estado inicial

            PrintStateMachine();
        }

        private void Update()
        {
            if (_currentStateModel != null)
            {
                // TODO: Pensar se o TryTransitions() deve ser chamado aqui ou apos o OnStay - Felipe acha melhor depois
                foreach (var action in _currentStateModel.OnStay)
                {
                    action.Execute(gameObject);
                }
            }
        }

        // TODO: enviar pra dentro da classe StateModel
        private bool TryTransitions()
        {
            if (_currentStateModel != null)
            {
                foreach (var transitionModel in _currentStateModel.Transitions)
                {
                    bool flag = true;
                    foreach (var condition in transitionModel.Conditions)
                    {
                        if (!condition.Evaluate(gameObject))
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        //SetState(transitionModel.To); TODO: obter o StateModel a partir da string "To"
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        private void SetState(StateModel state)
        {
            if (_currentStateModel != null)
            {
                foreach (var action in _currentStateModel.OnLeave)
                {
                    action.Execute(gameObject);
                }
                _currentStateModel = state;
                foreach (var action in _currentStateModel.BeforeEnter)
                {
                    action.Execute(gameObject);
                }

                bool hasChangedState = false; //TODO: = return do metodo "tentar transicao"
                if (!hasChangedState)
                {
                    foreach (var action in _currentStateModel.OnEnter)
                    {
                        action.Execute(gameObject);
                    }
                }
            }
        }

        private void PrintStateMachine()
        {
            print($"Initial: {_stateMachineModel.InitialState}\n");
            foreach (var st in _stateMachineModel.States)
            {
                print($"STATE: {st.Id}");

                if ((IList<ActionBase>)st.OnEnter is null || ((IList<ActionBase>)st.OnEnter).Count == 0)
                {
                }
                else
                {
                    print("  OnEnter");
                    foreach (var a in (IList<ActionBase>)st.OnEnter)
                        switch (a)
                        {
                            case LogAction log:
                                print($"    Log: {log.Message}");
                                break;
                            /*
                    case PlayAnimationAction anim:
                        print($"    PlayAnimation: {anim.Clip} loop={anim.Loop}");
                        break;
                    case WaitAction wait:
                        print($"    Wait: {wait.Seconds}s");
                        break;
                    */
                            default:
                                print($"    {a.GetType().Name}");
                                break;
                        }
                }

                if ((IList<ActionBase>)st.OnStay is null || ((IList<ActionBase>)st.OnStay).Count == 0)
                {
                }
                else
                {
                    print("  OnStay");
                    foreach (var a1 in (IList<ActionBase>)st.OnStay)
                        switch (a1)
                        {
                            case LogAction log1:
                                print($"    Log: {log1.Message}");
                                break;
                            /*
                    case PlayAnimationAction anim:
                        print($"    PlayAnimation: {anim.Clip} loop={anim.Loop}");
                        break;
                    case WaitAction wait:
                        print($"    Wait: {wait.Seconds}s");
                        break;
                    */
                            default:
                                print($"    {a1.GetType().Name}");
                                break;
                        }
                }

                if (st.Transitions != null)
                    foreach (var tr in st.Transitions)
                    {
                        print($"  → {tr.To}");
                        if ((IList<ConditionBase>)tr.Conditions is null || ((IList<ConditionBase>)tr.Conditions).Count == 0)
                        {
                        }
                        else
                        {
                            print("    if");
                            foreach (var c in (IList<ConditionBase>)tr.Conditions)
                                switch (c)
                                {
                                    /*
                    case MovingLeftCondition ml:
                        print($"      MovingLeft expr={ml.Expression}");
                        break;
                    case CrouchCondition cr:
                        print($"      Crouch pressed={cr.IsPressed}");
                        break;
                    */
                                    default:
                                        print($"      {c.GetType().Name}");
                                        break;
                                }
                        }
                    }
                print("------------");
            }
        }
    }
}