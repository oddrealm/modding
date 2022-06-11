using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Entities")]
public class GDEEntitiesData : ScriptableObject
{
	public string Key { get { return name; } }
	public string TooltipID = "";
	public int Size = 0;
	public EntityAgeTypes PermittedAges = EntityAgeTypes.ADULT;
	public EntityCompanionTypes CompanionType = 0;
	public EntityCompanionPermanenceTypes CompanionPermanenceType = 0;
	public EntityReproductionTypes ReproductionType = 0;
	public int ReproductionIntervalMinutes = 0;
	public int ReproductionCount = 0;
	public bool TakeLastNameOfCompanion = false;
	public int AgeToBecomeAdult = 0;
	public int AgeToBecomeElder = 0;
	public List<string> RandomElderStatuses = new List<string>();
	public string GenerateNameKey = "";
	public string InherentWorkAttacks = "";
	public string InherentCombatAttacks = "";
	public SkillTypes ProhibitedSkillTypes;
	public SkillTypes DefaultSkillTypes;
	public ProfessionTypes ProhibitedProfessionTypes;
	public ProfessionTypes PermittedRandomProfessions;
	public string Race = "";
	public InventoryTypes EquippableTypes = 0;
	public PathingTypes PathingType = 0;
	public string SleepPointTag = "";
	public string DefaultTuning = "";
	public string DeathItemSpawnGroup = "";
	public List<string> PreyRaces = new List<string>();
	public List<string> AttributeActions = new List<string>();
	public string DefaultPickUpBlueprint = "blueprint_pick_up_item";
	public string DefaultDropOffBlueprint = "blueprint_drop_off_item";
	public List<string> Statuses = new List<string>();
}
