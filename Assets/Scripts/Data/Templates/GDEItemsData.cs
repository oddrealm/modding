using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct ItemAction
{
    public string ActionID;
    public BuffData Buff;
    public string Status;
    public string SpawnID;
}

[System.Serializable]
public struct AutomatedItemActionActivation
{
    public string ActionID;
    public uint ActivationTime;
    public string LocationRequirementTagObjectID;
    public bool DisposeItem;
}

[System.Serializable]
public struct RandomEquipmentItem
{
    public string ItemID;
    public int ChanceToEquip; // i.e., ChanceToEquip / 1000; 0 = guaranteed
    public int ChanceToEquipMaxRoll; // i.e., 1 / MaxRoll
}

[CreateAssetMenu(menuName = "ScriptableObjects/Items")]
public class GDEItemsData : Scriptable, ISimulationData, IProgressionObject
{
    [Header("Usage Tags")]
    public string[] UsageTags = new string[0];
    [Header("Lifetime Minutes (-1 = disabled)")]
    public int MaxLifeTime = 0;
    [Header("Max amount to simulate for world gen")]
    public int MaxDefaultSimTime = 24 * 60 * 10;
    [Header("Simulation ID")]
    public string ItemSimulationID = "";
    [Header("Item Type (Tag)")]
    public string ItemType = "";
    public string OnJobDisposedTagObjectID = "";
    [Header("Tracking")]
    public bool ShowInTrackingByDefault = false;
    [Header("FX")]
    public string IdleFX = "";
    public string PickUpFX = "fx_block_item_picked_up";
    public string DropFX = "fx_block_item_dropped";
    [Header("SFX")]
    public string PickUpSFX = "";
    public string DropSFX = "";
    [Header("World Visuals")]
    public string Visuals = "";
    [Header("Entity Visuals")]
    public GDECharacterColorMaskData CharacterColorMask;
    public GDECharacterAccessoryData AccessoryData;
    public BlockPermissionTypes Permissions = BlockPermissionTypes.NONE;
    public BlockPermissionTypes Prohibited = BlockPermissionTypes.NONE;
    [Header("Set false for 2-hand weapons so that they don't allow additional attacks from other items.")]
    public bool CanStackAttacks = true;
    public bool FillAllSlots = false;
    public bool CannotBeUnequipped = false;
    public string[] PermittedSlots;
    public HashSet<string> PermittedSlotsHash = new HashSet<string>();
    public string AttackGroup = "";
    public int MerchantBuyValue = 0;
    public int MerchantSellValue = 0;
    [Header("Tuning set to < 0 will not be used for item generation. Use -1 for items meant to be unique/super rare.")]
    public int TuneRating = 0;
    public string ItemRarity = "item_rarity_common";
    public int DecayRate = 0;
    public string DecayItem = "";
    public int FlammableChance = 0;
    public int StackSize = 1;
    public ItemAction[] Actions = new ItemAction[0];
    public SimOptions[] StateOptions;
    public bool IsAvailableInCustomLoadout = false;

    public bool HasRangedAttack { get; private set; }
    public bool HasAttacks { get { return !string.IsNullOrEmpty(AttackGroup); } }
    public int MaxGlobalCount { get; private set; }
    public bool HasTimedActions { get { return TimedActions.Length > 0; } }
    public AutomatedItemActionActivation[] TimedActions = new AutomatedItemActionActivation[0];
    public Dictionary<string, AutomatedItemActionActivation[]> TimedActivatorsByActionID { get; private set; } = new Dictionary<string, AutomatedItemActionActivation[]>();
    public List<string> ActionIDs { get; private set; } = new List<string>();
    public Dictionary<string, ItemAction[]> ActionsByID { get; private set; } = new Dictionary<string, ItemAction[]>();
    public Dictionary<string, BuffData[]> BuffsByID { get; private set; } = new Dictionary<string, BuffData[]>();
    public Dictionary<string, string[]> StatusesByID { get; private set; } = new Dictionary<string, string[]>();
    public Dictionary<string, ITagObject[]> SpawnsByID { get; private set; } = new Dictionary<string, ITagObject[]>();
    public GDETagsData ItemTypeData { get; private set; }
    public GDETagsData ItemTagData { get; private set; }
    public GDEItemRarityData ItemRarityData { get; private set; }
    public GDETagsData[] UsageTagsData { get; private set; } = new GDETagsData[0];
    public override bool ShowMinimapCutoutColor { get { return false; } }
    public override Color MinimapColor
    {
        get
        {
            if (string.IsNullOrEmpty(Visuals)) { return Color.white; }

#if ODD_REALM_APP
            GDEBlockVisualsData blockVisuals = DataManager.GetTagObject<GDEBlockVisualsData>(Visuals);

            return blockVisuals.MapColor;
#else
            return Color.white;
#endif
        }
    }
    public string SimulationID { get { return ItemSimulationID; } }
    public GDESimulationData Simulation { get { return _simData; } }
    public SimTime MaxSimTime { get { return MaxDefaultSimTime; } }
    public bool CanShowInProgressUI
    {
        get
        {
            return true;
        }
    }
    public override bool IsNULL { get { return _isNull; } }
    public bool HasUtilityAction { get { return UtilityActions.Count > 0; } }
    public static HashSet<string> GlobalUtilityActionIDs { get; private set; } = new HashSet<string>()
    {
        "tag_can_use",
        "tag_can_drink",
        "tag_can_eat",
    };
    public List<string> UtilityActions { get; private set; } = new List<string>();
    public HashSet<string> UtilityActionsHash { get; private set; } = new HashSet<string>();

    private GDESimulationData _simData;
    private string[] _simStates = new string[]
    {
        ITEM_STATE_EXPIRED,
        ITEM_STATE_CAN_FALL
    };
    private bool _isNull = false;

    public const string ITEM_STATE_EXPIRED = "item_state_expired";
    public const string ITEM_STATE_CAN_FALL = "item_state_can_fall";

    public BuffData[] GetActionBuffs(string actionID)
    {
        if (!BuffsByID.TryGetValue(actionID, out var buffs))
        {
            return null;
        }

        return buffs;
    }

    public string[] GetActionStatuses(string actionID)
    {
        if (!StatusesByID.TryGetValue(actionID, out var statuses))
        {
            return null;
        }

        return statuses;
    }

    public ITagObject[] GetActionSpawns(string actionID)
    {
        if (!SpawnsByID.TryGetValue(actionID, out var spawns))
        {
            return null;
        }

        return spawns;
    }

    public void SetSimulationID(string simID)
    {
        ItemSimulationID = simID;
    }

    public override bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking()
        {
            TagID = ItemType,
            TagObjectID = Key,
            HideIfZero = true,
            StartDisabled = !ShowInTrackingByDefault,
            TrackingType = TrackingTypes.ITEM
        };

        return true;
    }

#if ODD_REALM_APP

    public string GetFirstUtilityActionText()
    {
        if (!HasUtilityAction) { return string.Empty; }

        for (int i = 0; i < UtilityActions.Count; i++)
        {
            string actionID = UtilityActions[i];

            if (string.IsNullOrEmpty(actionID)) { continue; }

            if (DataManager.TryGetTagObject(actionID, out var tagObj) && !string.IsNullOrEmpty(tagObj.TooltipAction))
            {
                return tagObj.TooltipAction;
            }
        }

        return string.Empty;
    }

    public bool IsHigherRarityThanOtherItem(GDEItemsData otherItem)
    {
        GDEItemRarityData ourRarity = DataManager.GetTagObject<GDEItemRarityData>(ItemRarity);
        GDEItemRarityData theirRarity = DataManager.GetTagObject<GDEItemRarityData>(otherItem.ItemRarity);

        return ourRarity.RarityScore > theirRarity.RarityScore;
    }

    public override void OnLoaded()
    {
        base.OnLoaded();

        if (!string.IsNullOrEmpty(ItemType))
        {
            ItemTypeData = DataManager.GetTagObject<GDETagsData>(ItemType);
        }

        ItemTagData = DataManager.GetTagObject<GDETagsData>("tag_item");
        ItemRarityData = DataManager.GetTagObject<GDEItemRarityData>(ItemRarity);
        _isNull = Key == "item_none";
        IsAvailableInCustomLoadout = ItemRarityData.IsAvailableInCustomLoadout;
        MaxGlobalCount = -1;

        switch (ItemRarity)
        {
            case "item_rarity_legendary":
                MaxGlobalCount = 1;
                break;

            default:
                MaxGlobalCount = 5_000;
                break;
        }

        if (Key == "item_ren")
        {
            MaxGlobalCount = -1;
        }

#if DEV_TESTING
        if (string.IsNullOrEmpty(Visuals))
        {
            Debug.LogError($"Visuals Key is empty for {Key}!");
        }

        if (CanStackAttacks && Key.Contains("two_hand"))
        {
            Debug.LogError($"{Key} is a two-hand item but CanStackAttacks is true!");
            CanStackAttacks = false;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        if (UsageTags.Length > 0)
        {
            UsageTagsData = new GDETagsData[UsageTags.Length];

            for (int i = 0; UsageTags != null && i < UsageTags.Length; i++)
            {
                if (string.IsNullOrEmpty(UsageTags[i]))
                {
                    continue;
                }

                UsageTagsData[i] = DataManager.GetTagObject<GDETagsData>(UsageTags[i]);
            }
        }

        ActionIDs.Clear();
        ActionsByID.Clear();
        BuffsByID.Clear();
        StatusesByID.Clear();
        SpawnsByID.Clear();
        UtilityActions.Clear();
        UtilityActionsHash.Clear();
        TimedActivatorsByActionID.Clear();

        for (int i = 0; i < TimedActions.Length; i++)
        {
            if (!TimedActivatorsByActionID.TryGetValue(TimedActions[i].ActionID, out var activators))
            {
                activators = new AutomatedItemActionActivation[1];
                activators[0] = TimedActions[i];
                TimedActivatorsByActionID.Add(TimedActions[i].ActionID, activators);
            }
            else
            {
                AutomatedItemActionActivation[] prevActions = activators;
                activators = new AutomatedItemActionActivation[activators.Length + 1];

                for (int j = 0; j < prevActions.Length; j++)
                {
                    activators[j] = prevActions[j];
                }

                activators[activators.Length - 1] = TimedActions[i];
                TimedActivatorsByActionID[TimedActions[i].ActionID] = activators;
            }
        }

        for (int i = 0; i < Actions.Length; i++)
        {
            string actionID = Actions[i].ActionID;

            if (string.IsNullOrEmpty(actionID) || !DataManager.TryGetTagObject<GDETagsData>(actionID, out var tagObj))
            {
#if DEV_TESTING
                Debug.LogError($"{Key} Item action ID cannot be empty!");
#endif
                continue;
            }

            if (GlobalUtilityActionIDs.Contains(actionID) && UtilityActionsHash.Add(actionID))
            {
                UtilityActions.Add(actionID);
            }

            bool hasOutput = false;

            if (TagIDsHash.Add(Actions[i].ActionID))
            {
                TagIDs.Add(Actions[i].ActionID);

#if DEV_TESTING && UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }

            if (!ActionsByID.TryGetValue(Actions[i].ActionID, out var actions))
            {
                ActionIDs.Add(Actions[i].ActionID);
                actions = new ItemAction[1];
                actions[0] = Actions[i];
                ActionsByID.Add(Actions[i].ActionID, actions);
            }
            else
            {
                ItemAction[] prevActions = actions;
                actions = new ItemAction[actions.Length + 1];

                for (int j = 0; j < prevActions.Length; j++)
                {
                    actions[j] = prevActions[j];
                }

                actions[actions.Length - 1] = Actions[i];
                ActionsByID[Actions[i].ActionID] = actions;
            }

            if (!Actions[i].Buff.IsNULL)
            {
                hasOutput = true;
                if (!BuffsByID.TryGetValue(Actions[i].ActionID, out var buffs))
                {
                    buffs = new BuffData[1];
                    buffs[0] = Actions[i].Buff;
                    BuffsByID.Add(Actions[i].ActionID, buffs);
                }
                else
                {
                    BuffData[] prevBuffs = buffs;
                    buffs = new BuffData[buffs.Length + 1];

                    for (int j = 0; j < prevBuffs.Length; j++)
                    {
                        buffs[j] = prevBuffs[j];
                    }

                    buffs[buffs.Length - 1] = Actions[i].Buff;
                    BuffsByID[Actions[i].ActionID] = buffs;
                }
            }

            if (!string.IsNullOrEmpty(Actions[i].Status))
            {
                hasOutput = true;
                if (!StatusesByID.TryGetValue(Actions[i].ActionID, out var statuses))
                {
                    statuses = new string[1];
                    statuses[0] = Actions[i].Status;
                    StatusesByID.Add(Actions[i].ActionID, statuses);
                }
                else
                {
                    string[] prevStatuss = statuses;
                    statuses = new string[statuses.Length + 1];

                    for (int j = 0; j < prevStatuss.Length; j++)
                    {
                        statuses[j] = prevStatuss[j];
                    }

                    statuses[statuses.Length - 1] = Actions[i].Status;
                    StatusesByID[Actions[i].ActionID] = statuses;
                }
            }

            if (!string.IsNullOrEmpty(Actions[i].SpawnID))
            {
                hasOutput = true;
                if (!SpawnsByID.TryGetValue(Actions[i].ActionID, out var spawns))
                {
                    spawns = new ITagObject[1];
                    spawns[0] = DataManager.GetTagObject(Actions[i].SpawnID);
                    SpawnsByID.Add(Actions[i].ActionID, spawns);
                }
                else
                {
                    ITagObject[] prevSpawns = spawns;
                    spawns = new ITagObject[spawns.Length + 1];

                    for (int j = 0; j < prevSpawns.Length; j++)
                    {
                        spawns[j] = prevSpawns[j];
                    }

                    spawns[spawns.Length - 1] = DataManager.GetTagObject(Actions[i].SpawnID);
                    SpawnsByID[Actions[i].ActionID] = spawns;
                }
            }

#if DEV_TESTING
            if (!hasOutput)
            {
                Debug.LogError($"{Key} Item action {actionID} has no output!");
            }
#endif
        }

        ActionIDs = ActionIDs.OrderBy(a => a).ToList();

        if (string.IsNullOrEmpty(ItemType) && Key != "item_none")
        {
            Debug.LogError(Key + " has no item type!");
        }

        PermittedSlotsHash.Clear();

        for (int i = 0; i < PermittedSlots.Length; i++)
        {
            PermittedSlotsHash.Add(PermittedSlots[i]);
        }

        if (!HasUtilityAction && PermittedSlotsHash.Contains("item_slot_utility"))
        {
            List<string> permittedSlots = new List<string>(PermittedSlots);
            permittedSlots.Remove("item_slot_utility");
            PermittedSlotsHash.Remove("item_slot_utility");
            PermittedSlots = permittedSlots.ToArray();
#if UNITY_EDITOR
            Debug.LogError($"{Key} has no utility action but has item_slot_utility in PermittedSlots! Removing it.");
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        else if (HasUtilityAction && !PermittedSlotsHash.Contains("item_slot_utility"))
        {
            List<string> permittedSlots = new List<string>(PermittedSlots);
            permittedSlots.Add("item_slot_utility");
            PermittedSlotsHash.Add("item_slot_utility");
            PermittedSlots = permittedSlots.ToArray();
#if UNITY_EDITOR
            Debug.LogError($"{Key} has utility actions but does not have item_slot_utility in PermittedSlots! Adding it.");
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        if (!string.IsNullOrEmpty(ItemSimulationID))
        {
            _simData = DataManager.GetTagObject<GDESimulationData>(ItemSimulationID);
        }

        HasRangedAttack = false;

        if (HasAttacks)
        {
            GDEAttackGroupsData attackGroup = DataManager.GetTagObject<GDEAttackGroupsData>(AttackGroup);

            for (int i = 0; i < attackGroup.Attacks.Count; i++)
            {
                GDEAttacksData attack = DataManager.GetTagObject<GDEAttacksData>(attackGroup.Attacks[i]);

                if (attack.IsRanged)
                {
                    HasRangedAttack = true;
                    break;
                }
            }
        }
    }
#endif

    public string[] GetSimStates()
    {
        return _simStates;
    }

    public SimOptions[] GetOptions()
    {
        return StateOptions;
    }
}
