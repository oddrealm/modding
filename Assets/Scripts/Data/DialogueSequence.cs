using UnityEngine;
using System.Collections.Generic;

public interface IDialogueSpeaker : ITooltipContent
{
    void ActivateDialogueOption(string optionID);
    void OnDialogueAction(DialogueAction action);
    int GetDialogueOptionCount();
    DialogueOption GetDialogueOption(int index);
    bool IsDialogueSequencePassed(SequenceCondition condition);
}

public struct DialogueOption
{
    public string DialogueID;
    public int Activations;
}

[System.Serializable]
public struct SequenceCondition
{
    public string TagRequirement0ID;
    public string TagRequirement1ID;
    public string TagRequirement2ID;
}

[System.Serializable]
public struct DialogueSequence
{
    public string ID;
    public string Text;
    public bool IsStartingSequence;
    public SequenceCondition Condition;
    public DialogueAction ActionA;
    public DialogueAction ActionB;
    public DialogueAction ActionC;
    public DialogueAction ActionD;
}

[System.Serializable]
public struct DialogueAction
{
    public string ID;
    public string Text;
    public string NextSequenceID;
    public DialogueActionTypes ActionTrigger;
    public string ActionDataID;
}

public enum DialogueActionTypes
{
    NONE,
    SPEAKER_ACTION,
}