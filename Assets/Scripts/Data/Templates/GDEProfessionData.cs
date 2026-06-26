using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Profession")]
public class GDEProfessionData : Scriptable
{
    [System.Serializable]
    public struct SkillPriority
    {
        public string SkillID;
        public int Priority;
    }

    public int MaxMembers = -1;
    public int OrderGroup = 0;
    public bool AutoCreateUniform = true;
    public string ResearchKey = "";
    public string RequiredRaceID = "";
    public string RequiredGenderID = "";
    public bool CanBeAssigned = true;
    public bool CanBeChanged = true;
    public bool FavoursRangedAttackItems = false;
    public bool IsCombatProfession = false;
    public bool IsLeader = false;
    public RandomEquipmentItem[] StartingItems = new RandomEquipmentItem[] { };
    public string[] SkillsActiveByDefault;
    public HashSet<string> SkillsActiveByDefaultHash = new();
    public bool PlayerCanEditLevelUpSkills = false;
    public int RandomSkillsToIncreaseOnLevelUp = 0;
    public string[] SkillsToIncreaseOnLevelUp;
    public HashSet<string> SkillsToIncreaseOnLevelUpHash = new();
    public string[] StatusesToAdd = System.Array.Empty<string>();

    public List<SkillPriority> SkillPriorities = new();
    [System.NonSerialized]
    public Dictionary<string, SkillPriority> SkillPrioritiesBySkillID = new();

    public int GetSkillPriority(string skillID)
    {
        if (SkillPrioritiesBySkillID.TryGetValue(skillID, out var skillPriority))
        {
            return skillPriority.Priority;
        }

#if DEV_TESTING
        Debug.LogError($"SkillPriority not found for {Key} skillID: {skillID}");
#endif

        return 1;
    }

    public override bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking()
        {
            TagID = "tag_professions",
            TagObjectID = Key,
            HideIfZero = true,
            StartDisabled = true,
        };

        return true;
    }

#if ODD_REALM_APP
    public override void OnReordered(int dataIndex)
    {
        base.OnReordered(dataIndex);
    }

    public override void SetOrderKey(string orderKey)
    {
        base.SetOrderKey(OrderGroup + orderKey);
    }


    public override void OnLoaded()
    {
        base.OnLoaded();
        EnsureTag("tag_professions");

        SkillPrioritiesBySkillID.Clear();

        for (int i = 0; i < SkillPriorities.Count; i++)
        {
            SkillPrioritiesBySkillID.Add(SkillPriorities[i].SkillID, SkillPriorities[i]);
        }

        SkillsActiveByDefaultHash.Clear();

        for (int i = 0; SkillsActiveByDefault != null && i < SkillsActiveByDefault.Length; i++)
        {
            SkillsActiveByDefaultHash.Add(SkillsActiveByDefault[i]);
        }

        SkillsToIncreaseOnLevelUpHash.Clear();

        for (int i = 0; SkillsToIncreaseOnLevelUp != null && i < SkillsToIncreaseOnLevelUp.Length; i++)
        {
            SkillsToIncreaseOnLevelUpHash.Add(SkillsToIncreaseOnLevelUp[i]);
        }
    }
#endif
}
