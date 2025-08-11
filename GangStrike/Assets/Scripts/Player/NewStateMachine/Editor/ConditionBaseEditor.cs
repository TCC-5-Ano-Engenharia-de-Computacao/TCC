// Editor/ConditionBaseEditor.cs
// (Opcional) Botão de avaliação manual para Conditions, útil pra depurar no Inspector
using UnityEditor;
using UnityEngine;
using Player.NewStateMachine.Conditions;
using StateMachine;

[CustomEditor(typeof(ConditionBase), /*inherit*/ true)]
[CanEditMultipleObjects]
public class ConditionBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(8);

        using (new EditorGUI.DisabledScope(!Application.isPlaying))
        {
            if (GUILayout.Button("Evaluate (Debug)"))
            {
                foreach (var t in targets)
                {
                    var cond = (ConditionBase)t;
                    if (cond == null) continue;


                    var ok = cond.Evaluate();
                    Debug.Log($"[{cond.name}] Evaluate => {ok}");
                }
            }
        }

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Entre em Play Mode para avaliar a Condition.", MessageType.Info);
        }
    }
}