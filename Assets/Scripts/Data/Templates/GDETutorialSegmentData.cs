using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TutorialSegment")]
public class GDETutorialSegmentData : Scriptable
{
    public string Comment = "TUTORIAL";

    [Header("On Completed")]
    public string NextSegment;
    [Header("Requirements")]
    public string PreviousSegment = "";
    public TutorialActivationTriggers Trigger;
    public string CustomEventID = "";
    public string EarlyCompleteCustomEvent = "";
    public int MinGameMinutes = -1;
    public float MinScaledPlayedTime = -1f;
    public float MinUnscaledPlayedTime = -1f;
    public float MinCooldown = -1;
    public string[] ActiveRaces;
    public string[] PermittedWindows;
    public SelectionTypes SelectionType = SelectionTypes.NONE;
    public string SelectionInputID = "";
    public SelectionTargetTypes SelectionTargetType = SelectionTargetTypes.NONE;
    public string[] AttributeRaces;
    public string AttributeType = "";
    public int MinAttributeAmount = -1;
    public int MaxAttributeAmount = -1;
    public string[] ItemIDs;
    public ConditionTypes ItemConditionType = ConditionTypes.OR;
    public int ItemMinCount = -1;
    public BlueprintCategories ConstructionCategoryType = BlueprintCategories.NONE;
    public int MinRoomCount = -1;
    public int MinProductionRoomCount = -1;
    public int MinAutoJobCount = -1;
    public int ProfessionCount = -1;
    public string ProfessionID = "";
    public string SkillID = "";

    [Header("Display")]
    public string Speaker = "Darby, the Historian";
    public string Icon = "sp_adult_darby_ma0";
    public string BackgroundMaskTarget = "";
    public TutorialMessage[] Message;


    public bool IsValid { get; private set; }

    public void SetPreviousSegment(string previousSegment)
    {
        if (previousSegment == PreviousSegment) { return; }

#if DEV_TESTING && UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.LogError($"Setting previous segment of {Key} to {previousSegment}");
#endif
        PreviousSegment = previousSegment;
    }

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        EnsureTag("tag_tutorial");

        if (!string.IsNullOrEmpty(NextSegment))
        {
            if (DataManager.TryGetTagObject(NextSegment, out GDETutorialSegmentData nextSegment))
            {
                nextSegment.SetPreviousSegment(Key);
            }
        }

        if (Message != null && Message.Length > 0)
        {
            for (int i = 0; i < Message.Length; i++)
            {
                if (string.IsNullOrEmpty(Message[i].Body)) { continue; }
                IsValid = true;
            }
        }

        base.OnLoaded();
    }
#endif
}

[System.Serializable]
public struct TutorialMessage
{
    public bool OnlyShowInputHotkey;
    public string InputID;
    public string TagObjectID;
    public string Body;
    public bool NewLine;
    public bool UseOverrideColor;
    public Color OverrideColor;
}

