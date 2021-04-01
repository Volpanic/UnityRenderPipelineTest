using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialougeSequence))]
public class DialougeEditor : Editor
{
    private DialougeSequence sequence = null;

    private void OnEnable()
    {
        sequence = (DialougeSequence)target;
    }

    public override void OnInspectorGUI()
    {
        if(sequence.DialougeLines != null)
        {
            // Loop through each line and display it
            for(int i = 0; i < sequence.DialougeLines.Count; i++)
            {
                DialougeLine line = sequence.DialougeLines[i];

                if(!DrawDialougeLine(ref line)) // If returns true, removes the dialouge line
                {
                    sequence.DialougeLines.RemoveAt(i);
                    i--;
                    EditorUtility.SetDirty(target); // Mark that the item has been edited, so it saves
                    Undo.RecordObject(target, "Removed Dialouge Line.");
                }

            }
        }

        //Add button
        if(GUILayout.Button("Add Line"))
        {
            if(sequence.DialougeLines == null) // Make sure the list exsits containing the lines
            {
                sequence.DialougeLines = new List<DialougeLine>();
            }

            sequence.DialougeLines.Add(new DialougeLine());
            EditorUtility.SetDirty(target); // Mark that the item has been edited, so it saves
        }
    }

    private bool DrawDialougeLine(ref DialougeLine line)
    {

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.BeginHorizontal(); // Line
            {
                //Speaker
                EditorGUI.BeginChangeCheck();
                line.SpeakerFace = EditorGUILayout.ObjectField("", line.SpeakerFace, typeof(Sprite), false, GUILayout.MaxWidth(64)) as Sprite;

                EditorStyles.textField.wordWrap = true;
                line.DialougeText = EditorGUILayout.TextArea(line.DialougeText, GUILayout.MaxHeight(64));

                if(EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Changed dialouge.");
                    EditorUtility.SetDirty(target);
                }

                //GUILayout.FlexibleSpace();

                EditorGUILayout.EndHorizontal();
            }

            //Draw remove button
            if(GUILayout.Button("Remove"))
            {
                EditorGUILayout.EndVertical(); // Run end premeturely before we return.
                return false;
            }


            EditorGUILayout.EndVertical();
        }

        return true;
    }
}
