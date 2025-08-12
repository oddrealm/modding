using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Blocks")]
public class GDEBlocksData : Scriptable, ISimulationData, IProgressionObject
{
    [System.Serializable]
    public struct Trigger
    {
        public string Comment;
        public string ID;
        public string ActiveTooltipID;
        public string InactiveTooltipID;
        public BlockPermissionTypes Permissions;
        public string Visuals;
        public string SFX;
        public string FX;
        public TriggerActionTypes Action;
        public string ActionTagObjectID;
        public bool DeactivateImmediate;
        public BuffData Buff;
    }

    [System.Serializable]
    public struct TriggerCondition
    {
        public string ActivateTriggerID;
        public string DeactivateTriggerID;
        public TriggerTypes TriggerType;
        public bool HideInTooltips;
        public bool Invert;
        public string SourceTagObjectRequirement0;
        public string SourceTagObjectRequirement1;
        public uint ElapsedTime;
    }

    [Header("Usage Tags")]
    public string[] UsageTags = System.Array.Empty<string>();
    [Header("Fill")]
    public string DefaultFill = "block_fill_air";
    [Header("Triggers")]
    public TriggerCondition[] TriggerConditions;
    public Trigger[] Triggers;
    [Header("Hidden blocks are revealed up to this distance.")]
    public int RevealDistance = 0;
    [Header("Temperature Source")]
    public int TemperatureSource = 0;
    [Header("Rotation")]
    public bool IsRotationFixture;
    public string RotationFixtureKey = string.Empty;
    [Header("Used by jobs to determine work xp reward and work progress.")]
    public int SkillLevel = 0;
    [Header("Items")]
    public int MaxItemCount = 0;
    public string[] ItemTypePermissions;
    [HideInInspector]
    public HashSet<string> ItemTypePermissionsHash = new HashSet<string>();
    public bool CanShowItems = false;
    public bool RestrictItemsToSameKey = false;
    public int DecayBuff = 0;
    public string RemoveItemDrops = "";
    [Header("Pathing")]
    public int VerticalEntityLift = 0;
    public bool IsObstruction;
    public bool IsVerticalAccess = false;
    public BlockDirectionFlags PermittedPathsAbove = BlockDirectionFlags.ALL;
    public BlockDirectionFlags PermittedPaths = BlockDirectionFlags.ALL;
    public float VerticalMovementMod = 1.0f;
    public float HorizontalMovementMod = 1.0f;
    public int MovementCost = 3;
    public bool VerticalMoveRequiresClimb = false;
    public bool ForceFocusDirection = false;
    [Header("Permissions/Prohibitions")]
    public BlockPermissionTypes Permissions = BlockPermissionTypes.NONE;
    public BlockPermissionTypes PermissionsAbove = BlockPermissionTypes.NONE;
    public BlockPermissionTypes Prohibited = BlockPermissionTypes.NONE;
    public BlockPermissionTypes ProhibitedAbove = BlockPermissionTypes.NONE;
    [Header("Layers")]
    public BlockLayers Layer = BlockLayers.BLOCK;
    public BlockLayers ProhibitedLayers = BlockLayers.PLATFORM;
    public bool CanHaveRoom = true;
    public bool CanHavePlatform = true;
    public BlockPlantTypes PermittedPlantTypes;
    public bool IsFertile = false;
    public bool CanSitAt = false;
    [Header("Lighting")]
    public bool IsSpotlight = false;
    [Header("Visuals")]
    public List<string> Visuals = new List<string>();
    public int TriggerOffsetX = 0;
    public int TriggerOffsetY = 0;
    public int HasItemsOffsetX = 0;
    public int HasItemsOffsetY = 0;
    public int FullCapacityOffsetX = 0;
    public int FullCapacityOffsetY = 0;
    [Header("FX")]
    public string IdleFX = "";
    public string UntriggeredFX = "";
    [Header("SFX")]
    public string AddSFX = "";
    public string RemoveSFX = "";
    public string InteractSFX = "";
    [Header("Lifetime Minutes (-1 = disabled)")]
    public int MaxLifeTime = 0;
    [Header("Max amount to simulate for world gen")]
    public int MaxDefaultSimTime = 24 * 60 * 10;
    [Header("Simulation ID")]
    public string BlockimulationID = "";
    public SimOptions[] StateOptions = System.Array.Empty<SimOptions>();
    [System.NonSerialized]
    public Dictionary<string, SimOptions> TagObjectsByOptionID = new Dictionary<string, SimOptions>();

    public bool IsNone { get; private set; }
    public bool CanShowInProgressUI { get { return true; } }
    public override bool ShowMinimapCutoutColor
    {
        get
        {
            if (Visuals.Count == 0) { return false; }

#if ODD_REALM_APP
            GDEBlockVisualsData blockVisuals = DataManager.GetTagObject<GDEBlockVisualsData>(Visuals[0]);

            return blockVisuals.ShowMinimapCutoutColor;
#else
            return false;
#endif
        }
    }
    public override bool ShowOnMinimap
    {
        get
        {
            return Visuals.Count > 0;
        }
    }
    public override Color MinimapColor
    {
        get
        {
            if (Visuals.Count == 0) { return Color.white; }

#if ODD_REALM_APP
            GDEBlockVisualsData blockVisuals = DataManager.GetTagObject<GDEBlockVisualsData>(Visuals[0]);

            return blockVisuals.MapColor;
#else
            return Color.white;
#endif
        }
    }
    public BlockRotationTypes RotationType { get; private set; }
    public string TagObjectID
    {
        get { return Key; }
    }
    public string TagObjectTooltipID
    {
        get { return TooltipID; }
    }
    public string SimulationID { get { return BlockimulationID; } }
    public GDESimulationData Simulation { get { return _simData; } }
    public SimTime MaxSimTime { get { return MaxDefaultSimTime; } }
    public GDEBlockFillData DefaultFillData { get; private set; }
    public GDETagsData[] UsageTagsData { get; private set; } = new GDETagsData[0];
    public GDETagsData[] TriggerTagsData { get; private set; } = new GDETagsData[0];

    private GDESimulationData _simData;
    private string[] _simStates = new string[]
    {
        BLOCK_STATE_CAN_FALL
    };

    public const string BLOCK_STATE_CAN_FALL = "block_state_can_fall";
    public const string TAG_HEAT_SOURCE = "tag_heat_source";

    public void SetSimulationID(string simID)
    {
        BlockimulationID = simID;
    }

    public string[] GetSimStates()
    {
        return _simStates;
    }

    public SimOptions[] GetOptions()
    {
        return StateOptions;
    }

#if ODD_REALM_APP
    private static HashSet<string> _tagIDCache = new HashSet<string>(4);
    public override void OnLoaded()
    {
        base.OnLoaded();

#if DEV_TESTING
        // for (int i = 0; i < TriggerConditions.Length; i++)
        // {
        //     if (string.IsNullOrEmpty(TriggerConditions[i].ActivateTriggerID))
        //     {
        //         Debug.LogError($"Missing ActivateTriggerID for TriggerCondition {i} on {Key}");
        //     }
        // }

        // for (int i = 0; i < Triggers.Length; i++)
        // {
        //     if (string.IsNullOrEmpty(Triggers[i].ActiveTooltipID)) { continue; }
        //
        //     if (string.IsNullOrEmpty(Triggers[i].InactiveTooltipID))
        //     {
        //         Debug.LogError($"Trigger {i} for {Key} - ID: {Triggers[i].ActiveTooltipID} does not have an inactive tooltip, setting to active tooltip.");
        //     }
        // }
#endif

        IsNone = Key == "block_none";

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

        if (Triggers != null && Triggers.Length > 0)
        {
            _tagIDCache.Clear();

            for (int i = 0; i < Triggers.Length; i++)
            {
                if (string.IsNullOrEmpty(Triggers[i].ID))
                {
                    continue;
                }

                _tagIDCache.Add(Triggers[i].ID);

                if (TagIDsHash.Add(Triggers[i].ID))
                {
                    TagIDs.Add(Triggers[i].ID);
                }
            }

            if (_tagIDCache.Count > 0)
            {
                TriggerTagsData = new GDETagsData[_tagIDCache.Count];
                int i = 0;

                foreach (string tagID in _tagIDCache)
                {
                    TriggerTagsData[i] = DataManager.GetTagObject<GDETagsData>(tagID);
                    i++;
                }

                _tagIDCache.Clear();
            }
        }

#if DEV_TESTING
        if (TemperatureSource > 0 && TagIDsHash.Add(TAG_HEAT_SOURCE))
        {
            TagIDs.Add(TAG_HEAT_SOURCE);
            Debug.LogError($"Adding heat source tag to: {Key}");
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
#endif

        if (!IsObstruction && PermittedPaths == BlockDirectionFlags.NONE)
        {
            Debug.LogError($"{Key} has no permitted paths and is not obstruction, setting to allow ALL!");
            PermittedPaths = BlockDirectionFlags.ALL;
        }

        RotationType = BlockRotationTypes.NONE;

        for (int i = 0; i < Visuals.Count; i++)
        {
            GDEBlockVisualsData visuals = DataManager.GetTagObject<GDEBlockVisualsData>(Visuals[i]);
            RotationType |= visuals.RotationType;
        }

        ItemTypePermissionsHash.Clear();

        for (int i = 0; ItemTypePermissions != null && i < ItemTypePermissions.Length; i++)
        {
            ItemTypePermissionsHash.Add(ItemTypePermissions[i]);
        }

        if (!string.IsNullOrEmpty(SimulationID))
        {
            _simData = DataManager.GetTagObject<GDESimulationData>(SimulationID);
        }

        DefaultFillData = DataManager.GetTagObject<GDEBlockFillData>(DefaultFill);
    }
#endif

}
