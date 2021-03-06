using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Blueprints")]
public class GDEBlueprintsData : ScriptableObject
{
	public string Key;

	[Header("Progression Start")]
	public JobProgressTypes StartProgressType = JobProgressTypes.FIXED_AMOUNT;
	public AttributeTypes StartProgressAttribute = AttributeTypes.NONE;
	public float StartProgressAttributePercent = 1f;
	public int FixedProgressStart = 0;

	[Header("Progression Target")]
	public JobProgressTypes TargetProgressType = JobProgressTypes.FIXED_AMOUNT;
	public AttributeTypes TargetProgressAttribute = AttributeTypes.NONE;
	public float TargetProgressAttributePercent = 1f;
	public int FixedProgressTarget = 100;

	[Header("")]
	public int DefaultAutoJobPriority = 1;
	public ThresholdTypes DefaultThresholdType = ThresholdTypes.AUTO;
	public bool FreeIfInaccessible = false;
	public float TimeoutTime = 10f;
	public string Requirements = "";
	public AutoJobBlockSearchTypes AutoJobBlockSearchType = AutoJobBlockSearchTypes.FIRST_ROOM_POINT;
	public JobBarTypes JobBarType = JobBarTypes.NONE;
	public bool InvertProgressDisplay = false;
	public JobActionTextTypes JobActionTextTypes = JobActionTextTypes.NONE;

	public int Toughness = 0;
	public float XPMod = 0.0f;
	public bool UseBlockForToughness = false;
	public string ToolbarMenu = "";
	public string ResearchKey = "";
	public BlueprintPlacementTypes AutoPlacementClearType = BlueprintPlacementTypes.NONE;
	public BlueprintPlacementTypes Placement = BlueprintPlacementTypes.NONE;
	public BlueprintPlacementTypes PlacementClear = BlueprintPlacementTypes.NONE;
	public BlueprintCategories Category = BlueprintCategories.NONE;
	public string TooltipID = "";
	public bool OnlyShowYieldTooltip = false;
	public List<string> YieldTooltipIDs = new List<string>();
	public string InterfaceInfo = "";
	public List<string> Visuals = new List<string>();
	public string WorkFX = "";
	public string InteractSFX = "";
	public string CompleteSFX = "";
	public bool SetLiquidMass = false;
	public float LiquidMass = 1f;
	public int ModelIndex = 0;
	public int ModelMultipler = 0;
	public string EntitiesToCreate = "";
	public PickUpActionTypes PickUpActionType = PickUpActionTypes.NONE;
	public bool ResetPlantMaintenance = false;
	public bool ReleaseContainedEntity = false;
	public bool TryCaptureContainedEntity = false;
	public List<string> StatusesToApply = new List<string>();
	public List<string> StatusesToApplyToTarget = new List<string>();

	public bool MarkRequirementsDiscoveredOnDiscovery = true;

	public void Clone(GDEBlueprintsData other)
    {
		if (other == null) { return; }

		Requirements = other.Requirements;
		ModelMultipler = other.ModelMultipler;
		WorkFX = other.WorkFX;
		JobBarType = other.JobBarType;
		InvertProgressDisplay = other.InvertProgressDisplay;
		JobActionTextTypes = other.JobActionTextTypes;
		FixedProgressTarget = other.FixedProgressTarget;
		Toughness = other.Toughness;
		XPMod = other.XPMod;
		UseBlockForToughness = other.UseBlockForToughness;
		Visuals = new List<string>(other.Visuals);
		ToolbarMenu = other.ToolbarMenu;
		ResearchKey = other.ResearchKey;
		TooltipID = other.TooltipID;
		InterfaceInfo = other.InterfaceInfo;
		AutoPlacementClearType = other.AutoPlacementClearType;
		Placement = other.Placement;
		PlacementClear = other.PlacementClear;
		Category = other.Category;
		InteractSFX = other.InteractSFX;
		CompleteSFX = other.CompleteSFX;
		ModelIndex = other.ModelIndex;
		EntitiesToCreate = other.EntitiesToCreate;
		PickUpActionType = other.PickUpActionType;
		ResetPlantMaintenance = other.ResetPlantMaintenance;
		ReleaseContainedEntity = other.ReleaseContainedEntity;
		TryCaptureContainedEntity = other.TryCaptureContainedEntity;
	}
}
