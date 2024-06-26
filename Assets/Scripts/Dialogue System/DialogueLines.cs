using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueLines : ScriptableObject
{
    [SerializeField, TextArea]
    private string[] lines;
    [SerializeField]
    private string characterName;
    [SerializeField]
    private Sprite[] faces;
    [SerializeField]
    private Sprite frame;
}
