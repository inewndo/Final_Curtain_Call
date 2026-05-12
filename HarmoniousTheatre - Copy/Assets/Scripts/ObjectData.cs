using UnityEngine;
[CreateAssetMenu(menuName = "Object Data")]

public class ObjectData : ScriptableObject
{
    
    [Header("Speaker")]
    public string displayName;

    [Header("Dialogue")]
    [TextArea(3, 10)]
    public string[] lines;

    [Header("if there are no choices, we show buttons after lines end")]
    public DialogueChoice[] choices;

    [Header("if there are no choices, auto continue to next node")]
    public ObjectData nextNode;

    
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public ObjectData nextNode;
}
