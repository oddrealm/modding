using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityTuning")]
public class GDEEntityTuningData : ScriptableObject
{
	public string Key;
	public int SightRange = 0;
	public int MinRandomSkill = 0;
	public int MaxRandomSkill = 0;
	public SkillTuning[] SkillBonuses;
	public string Health = "";
	public int HealthChange = 0;
	public string Toughness = "";
	public string Evasion = "";
	public int MoveSpeed = 0;
	public int ClimbSpeed = 0;
	public int Happiness = 0;
	public int HappinessChange = 0;
	public int Hunger = 0;
	public int HungerChange = 0;
	public int Thirst = 0;
	public int ThirstChange = 0;
	public int Energy = 0;
	public int EnergyChange = 0;
	public int BlockCurrentTemperature = 0;
	public int Temperature = 0;
	public int ColdTolerance = 0;
	public int CritRate = 0;
	public int DamageIgnoreRate = 0;
	public int Age = 0;
	public int XP = 0;
	public int Level = 0;
	public int AttackSpeedMod = 0;
	public int OutputQualityMax = 0;
	public int FishLure = 0;
	public int FishScare = 0;
	public int ProjectileMultRate = 0;
	public int ProjectileMultMod = 0;
	public int UsageDecreaseRate = 0;
	public int UsageDecreaseMod = 0;
	public int ResourceFindRate = 0;
	public int ResourceFindMod = 0;
	public int BurnRate = 0;
	public int BurnMod = 0;
	public int Oxygen = 0;
	public int OxygenChange = 0;
	public List<string> StartingStatuses = new List<string>();
	public List<string> RandomStartingStatuses = new List<string>();
	public string FirstName = "";
	public string LastName = "";
	public string Appearance = "";
	public string InventoryGroupID = "";
	public string EquipmentGroupID = "";

	[System.Serializable]
	public class SkillTuning
    {
		public SkillTypes Skill;
		public int Amount;
    }
}
