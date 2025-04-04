using Assets.GameData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatusActionActivation
{
    ADD = 0,
    REMOVE = 1,
    NONE = 2
}

public enum StatusActionDeactivation
{
    REMOVE = 0,
    ADD = 1,
    NONE = 2
}

public enum StatusActionTypes
{
    NONE = 0,
    ADD_BUFFS = 1,
    ADD_STATUSES = 2,
    SPAWN_TAG_OBJECT = 3,
    REMOVE_BUFFS = 4,
    REMOVE_STATUSES = 5
}

[System.Serializable]
public struct StatusAction
{
    public string Comment;
    [Header("Skill, Attribute, Etc. - Leave empty if there is no condition requirement.")]
    public string ConditionTarget;
    public bool HideInTooltips;
    [Header("Used to activate from external sources (Optional). i.e., sleep job activates 'tag_can_sleep_in_bed' status action.")]
    public string ActivationID;
    [SerializeReference]
    public ICondition[] Conditions;
    [Header("Min Amount Of Elapsed Time To Activate (in minutes). i.e., Every 60 minutes.")]
    public uint TimeThresholdMinutes;
    [Header("Leave 0 to ignore. (1/100,000 roll)")]
    public uint RandomChance;
    public StatusActionTypes ActivationAction;
    public StatusActionTypes DeactivationAction;
    public BuffData[] Buffs;
    public string[] Statuses;
    public string SpawnID;
    public int SpawnMin;
    public int SpawnMax;

#if ODD_REALM_APP
    public ITagObject ConditionTargetObj { get { return string.IsNullOrEmpty(ConditionTarget) ? Scriptable.NULL_TAG_OBJECT : DataManager.GetTagObject(ConditionTarget); } }

    public bool HasConditions
    {
        get
        {
            if (ConditionTargetObj != null &&
                Conditions != null &&
                Conditions.Length > 0) { return true; }
            if (TimeThresholdMinutes > 0) { return true; }

            return false;
        }
    }

    public bool AreConditionsPassed(float current, float max)
    {
        if (Conditions == null || Conditions.Length == 0) { return true; }

        for (int i = 0; i < Conditions.Length; i++)
        {
            if (!Conditions[i].IsPassed(current, max))
            {
                return false;
            }
        }

        return true;
    }
#endif
}

public interface ICondition
{
    bool IsPassed(float current, float max);
}

[System.Serializable]
public struct EqualCondition : ICondition
{
    public float Amount;
    public bool Normalized;

    public bool IsPassed(float current, float max)
    {
        if (Normalized) current = (max > 0f ? (current / max) : 0f);
        return Mathf.Approximately(current, Amount);
    }
}

[System.Serializable]
public struct NotEqualCondition : ICondition
{
    public float Amount;
    public bool Normalized;

    public bool IsPassed(float current, float max)
    {
        if (Normalized) current = (max > 0f ? (current / max) : 0f);
        return !Mathf.Approximately(current, Amount);
    }
}

[System.Serializable]
public struct LessThanCondition : ICondition
{
    public float Amount;
    public bool Normalized;

    public bool IsPassed(float current, float max)
    {
        if (Normalized) current = (max > 0f ? (current / max) : 0f);
        return current < Amount;
    }
}

[System.Serializable]
public struct LessThanEqualCondition : ICondition
{
    public float Amount;
    public bool Normalized;

    public bool IsPassed(float current, float max)
    {
        if (Normalized) current = (max > 0f ? (current / max) : 0f);
        return current <= Amount;
    }
}

[System.Serializable]
public struct GreaterThanCondition : ICondition
{
    public float Amount;
    public bool Normalized;

    public bool IsPassed(float current, float max)
    {
        if (Normalized) current = (max > 0f ? (current / max) : 0f);
        return current > Amount;
    }
}

[System.Serializable]
public struct GreaterThanEqualCondition : ICondition
{
    public float Amount;
    public bool Normalized;

    public bool IsPassed(float current, float max)
    {
        if (Normalized) current = (max > 0f ? (current / max) : 0f);
        return current >= Amount;
    }
}

[CreateAssetMenu(menuName = "ScriptableObjects/EntityStatus")]
public class GDEEntityStatusData : Scriptable
{
    public bool TrackByDefault = false;
    public bool VisibleToPlayer = true;
    public bool ShowIndicator = false;
    public string Notification = "";
    public int ExpireTimeMinutesMin = 0;
    public int ExpireTimeMinutesMax = 0;
    public List<string> StatusesToRemove = new List<string>();
    public List<string> StatusesToAddOnExpire = new List<string>();
    public List<string> PermittedGenders = new List<string>();
    public List<string> PermittedFactions = new List<string>()
    {
        "faction_player",
        "faction_neutral",
        "faction_hostile",
        "faction_captured"
    };
    public List<string> StatusesToProhibit = new List<string>();
    public List<string> StatusesToDisable = new List<string>();
#if ODD_REALM_APP
    public Color TooltipBackgroundColor = UIPretty.UI_GOOD_GREEN_COLOR;
#else
    public Color TooltipBackgroundColor = Color.green;
#endif
    public string ColorMask;
    public string Accessory;
    public string IdleFXGroup = "";
    [Header("Actions change attributes and can be set on timers.")]
    public StatusAction[] Actions = new StatusAction[0];
    [System.NonSerialized]
    public List<int> SortedActionIndices = new List<int>();

    public List<StatusAutoJob> AutoJobs = new List<StatusAutoJob>();

    public string[] DialogueOptions = new string[0];

    public bool CanExpire { get { return ExpireTimeMinutesMax > 0; } }
    public HashSet<string> PermittedGendersHash { get; private set; }
    public HashSet<string> PermittedFactionsHash { get; private set; }

    public override bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking()
        {
            TagID = "tag_statuses",
            TagObjectID = Key,
            HideIfZero = true,
            StartDisabled = !TrackByDefault,
            TrackingType = TrackingTypes.ENTITY
        };

        return true;
    }

    [System.Serializable]
    public struct StatusAutoJob
    {
        public string BlueprintID;
        public AutoJobSettings Settings;
    }

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        PermittedGendersHash = new HashSet<string>(PermittedGenders);
        PermittedFactionsHash = new HashSet<string>(PermittedFactions);
        SortedActionIndices.Clear();

        if (Actions != null && Actions.Length > 0)
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                SortedActionIndices.Add(i);
            }

            // Sort actions by activation ID.
            SortedActionIndices = SortedActionIndices.OrderBy(a => Actions[a].ActivationID).ToList();
        }

        base.OnLoaded();
    }
#endif
}
