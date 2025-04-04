using UnityEngine;

public interface IDialogueSpeaker : ITooltipContent
{
    void ActivateDialogueOption(string optionID);
    void OnDialogueAction(DialogueAction action);
    int GetDialogueOptionCount();
    DialogueOption GetDialogueOption(int index);
    bool IsDialogueSequencePassed(SequenceCondition condition);
    LocationUID LocationUID { get; }
    InstanceUID UID { get; }
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

    public bool IsValid
    {
        get
        {
            return !string.IsNullOrEmpty(TagRequirement0ID) ||
                !string.IsNullOrEmpty(TagRequirement1ID) ||
                !string.IsNullOrEmpty(TagRequirement2ID);
        }
    }
}

[System.Serializable]
public class DialogueSequence
{
    public const string RANDOM_NEW_SPEAKER = "random_new_speaker";
    public const string SPEAKER0 = "speaker0";
    public const string SPEAKER1 = "speaker1";
    public const string SPEAKER2 = "speaker2";
    [Header("Used by actions to find the next sequence to start.")]
    public string ID;
    [Header("Dialogue text body.")]
    public string Text;
    public bool OverrideSpeaker;
    public string OverrideSpeakerID = SPEAKER0;
    public SequenceCondition Condition;
    public DialogueAction ActionA;
    public DialogueAction ActionB;
    public DialogueAction ActionC;
    public DialogueAction ActionD;
}

[System.Serializable]
public struct DialogueAction
{
    public string Text;
    public string NextSequenceID;
    public DialogueActionTypes ActionType;
    public SpeakerActionTypes SpeakerAction;
    public string ActionDataID;
    public int ActionCount;
    public string ScenarioID;

    public bool IsValid
    {
        get
        {
            return !string.IsNullOrEmpty(Text);
        }
    }
}

public enum DialogueActionTypes
{
    NONE,
    SPEAKER_ACTION,
    ALL_SPEAKERS_ACTION,
    ACTIVATE_SCENARIO,
}

public enum SpeakerActionTypes
{
    NONE,
    LEAVE_MAP,
    SET_FACTION
}