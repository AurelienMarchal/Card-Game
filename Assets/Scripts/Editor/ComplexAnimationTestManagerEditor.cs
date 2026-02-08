using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ComplexAnimationTestManager))]
public class ComplexAnimationTestManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ComplexAnimationTestManager complexAnimationTestManager = (ComplexAnimationTestManager)target;
        var anim = complexAnimationTestManager.complexAnimation;

        DrawDefaultInspector(); // draws serialized fields if any

        if (anim)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Runtime State", EditorStyles.boldLabel);

            // --- Read only fields ---
            GUI.enabled = false;

            EditorGUILayout.IntField("Step", anim.step);
            EditorGUILayout.Toggle("Step Finished", anim.StepFinished());
            EditorGUILayout.Toggle("Is Playing", anim.isPlaying);

            GUI.enabled = true;

            EditorGUILayout.Space(10);

            // --- Buttons ---
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("◀ Back"))
            {
                Undo.RecordObject(anim, "Back Step");
                anim.step = anim.step - 1;
            }

            if (GUILayout.Button("Play Step"))
            {
                anim.PlayStep();
            }

            if (GUILayout.Button("Next ▶"))
            {
                anim.NextStep();
            }

            EditorGUILayout.EndHorizontal();
        }

        // repaint while playing so values update live
        if (Application.isPlaying)
            Repaint();
    }
}
