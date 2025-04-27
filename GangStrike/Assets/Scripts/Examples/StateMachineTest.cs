using System;
using System.Collections.Generic;
using System.IO;
using StateMachine.Actions;
using StateMachine.Conditions;
using StateMachine.Serialization;
using UnityEngine;

namespace Examples
{
    class StateMachineTest:MonoBehaviour
    {
        private void Start()
        {
            PrintStateMachine();
            
        }

        void ParseStateMachine()
        {
            
        }

        void PrintStateMachine()
        {
            const string file = "Assets/Scripts/Examples/state_machine.xml"; // coloque seu caminho aqui
            var serializer = StateMachineSerializerFactory.Get();
            using var fs = File.OpenRead(file);
            var _stateMachineModel = (StateMachine.Model.StateMachineModel)serializer.Deserialize(fs);

            print($"Initial: {_stateMachineModel.InitialState}\n");
            foreach (var st in _stateMachineModel.States)
            {
                print($"STATE: {st.Id}");

                DumpActions("  OnEnter", st.OnEnter);
                DumpActions("  OnStay",  st.OnStay);

                if (st.Transitions != null)
                    foreach (var tr in st.Transitions)
                    {
                        print($"  → {tr.To}");
                        DumpConditions("    if", tr.Conditions);
                    }
                print("------------");
            }
        }

        static void DumpActions(string label, IList<ActionBase> list)
        {
            if (list is null || list.Count == 0) return;
            print(label);
            foreach (var a in list)
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

        static void DumpConditions(string label, IList<ConditionBase> list)
        {
            if (list is null || list.Count == 0) return;
            print(label);
            foreach (var c in list)
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
}