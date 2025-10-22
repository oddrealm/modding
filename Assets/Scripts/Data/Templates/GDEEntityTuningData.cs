using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityTuning")]
public class GDEEntityTuningData : Scriptable
{
    [Header("AGES:")]
    [Header("Exponential Random Age Generation")]
    public int Age = -1;
    [Header("PROFESSIONS:")]
    public List<string> RandomProfessions = new();
    [Header("SKILLS:")]
    [Header("Exponential Random Skill Generation")]
    public int MaxRandomSkill = 0;
    public int MinRandomSkill = 0;
    public SkillTuning[] SkillBonuses;
    [Header("STATUSES:")]
    public List<string> StartingStatuses = new();
    public List<string> RandomStartingStatuses = new();
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

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        for (int n = 0; n < SkillBonuses.Length; n++)
        {
            if (DataManager.TagObjectExists(SkillBonuses[n].Skill)) { continue; }

            Debug.LogError($"{Key} skill tuning not found for: {SkillBonuses[n].Skill}");
        }

        base.OnLoaded();
    }
#endif
}
