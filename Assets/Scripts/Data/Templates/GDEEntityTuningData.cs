using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityTuning")]
public class GDEEntityTuningData : Scriptable
{
    [Header("AGES:")]
    [Header("Exponential Random Age Generation")]
    public int MaxRandomAge = 100;
    public int MinRandomAge = 18;
    [Header("LEVELS:")]
    [Header("If faction != player, Exponential Random LVL Generation")]
    public int MaxRandomLevel = 5;
    public int MinRandomLevel = 1;
    [Header("PROFESSIONS:")]
    public List<string> RandomProfessions = new List<string>();
    [Header("SKILLS:")]
    [Header("Exponential Random Skill Generation")]
    public int MaxRandomSkill = 0;
    public int MinRandomSkill = 0;
    public SkillTuning[] SkillBonuses;
    [Header("STATUSES:")]
    public List<string> StartingStatuses = new List<string>();
    public List<string> RandomStartingStatuses = new List<string>();
    public string FirstName = "";
    public string LastName = "";
    public string Appearance = "";
    [Header("INVENTORY:")]
    public string InventoryGroupID = "";
    public bool AutoGenerateInventory = false;
    [Header("EQUIPMENT:")]
    public string EquipmentGroupID = "";
    public bool AutoGenerateEquipment = false;

    [System.Serializable]
    public class SkillTuning
    {
        public string Skill;
        public int Amount;
    }
}
