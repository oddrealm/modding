using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Entities")]
public class GDEEntitiesData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public int Size = 0;
	//public EntityAgeTypes PermittedAges = EntityAgeTypes.ADULT;
	public EntityCompanionTypes CompanionType = 0;
	public EntityCompanionPermanenceTypes CompanionPermanenceType = 0;
	public EntityReproductionTypes ReproductionType = 0;
	public int ReproductionIntervalMinutes = 0;
	public int ReproductionCount = 0;
	public bool TakeLastNameOfCompanion = false;
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

	public List<AgeInfo> PermittedAges = new List<AgeInfo>()
	{
		new AgeInfo() {
			AgeType = EntityAgeTypes.CHILD,
			Statuses = new List<string>{ "entity_status_child" },
			Min = 0,
			Max = 15,
			CanRespec = false
		},
		new AgeInfo() { 
			AgeType = EntityAgeTypes.ADULT,  
			Statuses = new List<string>{ "entity_status_adult" },
			Min = 16,
			Max = 60,
			CanRespec = true
		},
		new AgeInfo() {
			AgeType = EntityAgeTypes.ELDER,
			Statuses = new List<string>{ "entity_status_elder" },
			Min = 61,
			Max = 90,
			CanRespec = true
		}
	};

	[System.Serializable]
	public class AgeInfo
    {
		public int Min = 0;
		public int Max = 99;
		public bool CanRespec = true;
		public EntityAgeTypes AgeType = EntityAgeTypes.NONE;
		public List<string> Statuses = new List<string>();
		public List<AgeStatuses> RandomStatuses = new List<AgeStatuses>();
	}

	[System.Serializable]
	public class AgeStatuses
    {
		public int Chances = 10000;
		public string StatusID;
		public bool AddAgeToChances = true;
    }
}
