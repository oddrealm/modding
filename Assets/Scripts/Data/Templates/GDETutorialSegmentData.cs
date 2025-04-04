using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TutorialSegment")]
public class GDETutorialSegmentData : Scriptable
{
    public string Speaker = "Darby, the Historian";

    [Header("Requirements")]
    public string PreviousSegment;
    public TutorialActivationTriggers Trigger;
    public int MinGameMinutes = -1;
    public float MinScaledPlayedTime = -1f;
    public float MinUnscaledPlayedTime = -1f;
    public float MinCooldown = -1;
    public string[] Hooks;
    public string[] ActiveRaces;
    public string[] PermittedWindows;
    public SelectionTypes SelectionType = SelectionTypes.NONE;
    public string SelectionInputID = "";
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

    [Header("Pop-Up Settings")]
    public float FadeInDelay = 0f;
    public PanelPositions PopUpPosition;
    public bool ShowBackground;
    public bool ShowBackgroundMask;
    public string BackgroundMaskTarget;
    public string AnimatedImage;
    public string PopUpTextBody;

    public string Comment = "TUTORIAL";
    public string Icon = "sp_adult_darby_ma0";

    public TutorialMessage[] Message;

    [Header("On Completed")]
    public string NextSegment;

    public bool IsValid { get; private set; }

#if ODD_REALM_APP
    public override void OnLoaded()
    {
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

