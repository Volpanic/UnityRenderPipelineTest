using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialougeLine // Stores data for each dialouge line
{
    public string DialougeText;
    public Sprite SpeakerFace;
}

[CreateAssetMenu(fileName = "NewDialouge",menuName ="Dialouge",order = 0)]
public class DialougeSequence : ScriptableObject
{
    [SerializeReference]
    public List<DialougeLine> DialougeLines;
}