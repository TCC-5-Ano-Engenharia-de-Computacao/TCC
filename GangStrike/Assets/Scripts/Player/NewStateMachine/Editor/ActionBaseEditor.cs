// Editor/ActionBaseEditor.cs
// Mostra um botão "Execute (Debug)" para TODAS as Actions (herdado de ActionBase)
using UnityEditor;
using UnityEngine;
using Player.NewStateMachine.Actions;

[CustomEditor(typeof(ActionBase), /*inherit*/ true)]
[CanEditMultipleObjects]
public class ActionBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(8);

        using (new EditorGUI.DisabledScope(!Application.isPlaying))
        {
            if (GUILayout.Button("Execute (Debug)"))
            {
                foreach (var t in targets)
                {
                    var action = (ActionBase)t;
                    if (action == null) continue;
                    action.Execute();
                }
            }
        }

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Entre em Play Mode para executar a Action.", MessageType.Info);
        }
    }
}