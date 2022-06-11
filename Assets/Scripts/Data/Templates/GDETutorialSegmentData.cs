using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GDETutorialSegmentData : ScriptableObject
{
    public TutorialTypes TutorialType = TutorialTypes.POPUP;

    [Header("On Activate")]
    public bool SelectTarget = false;

    [Header("On Started")]
    public InputStateTypes InputsToLock = InputStateTypes.NON_SYS;

    [Header("Requirements")]
    public TutorialActivationTriggers Trigger;
    public GDETutorialSegmentData[] PreviousSegments;
    public int MinGameMinutes = -1;
    public float MinPlayTime = -1f;
    public float MinCooldown = -1;
    public string[] Hooks;
    public string[] ActiveRaces;
    public string[] PermittedWindows;
    public SelectionTypes SelectionType = SelectionTypes.NONE;
    public string[] AttributeRaces;
    public AttributeTypes AttributeType = AttributeTypes.NONE;
    public int MinAttributeAmount = -1;
    public int MaxAttributeAmount = -1;
    public string[] ItemIDs;
    public ConditionTypes ItemConditionType = ConditionTypes.OR;
    public int ItemMinCount = -1;
    public BlueprintCategories ConstructionCategoryType = BlueprintCategories.NONE;

    public int MinRoomCount = -1;
    public int MinProductionRoomCount = -1;
    public int MinAutoJobCount = -1;

    [Header("Dialogue Settings")]
    public GDEDialogueData Dialogue;

    [Header("Pop-Up Settings")]
    public float FadeInDelay = 0f;
    public PanelPositions PopUpPosition;
    public bool ShowBackground;
    public bool ShowBackgroundMask;
    public string BackgroundMaskTarget;
    public string AnimatedImage;
    public string PopUpTextBody;

    [Header("On Completed")]
    public GDETutorialSegmentData NextSegment;
}
