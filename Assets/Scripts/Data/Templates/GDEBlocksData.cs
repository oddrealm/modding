using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Blocks")]
public class GDEBlocksData : Scriptable, ISimulationData
{
    [System.Serializable]
    public struct Trigger
    {
        public string Comment;
        public string TooltipID;
        public string ID;
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
        public TriggerTypes TriggerType;
        public bool HideInTooltips;
        public bool Invert;
        public string ActivateTriggerID;
        public string DeactivateTriggerID;
        public string SourceTagObjectRequirement0;
        public string SourceTagObjectRequirement1;
        public uint ElapsedTime;
    }


    [Header("Fill")]
    public string DefaultFill = "block_fill_air";
    [System.NonSerialized]
    public GDEBlockFillData DefaultFillData;

    [Header("Triggers")]
    public TriggerCondition[] TriggerConditions;
    public Trigger[] Triggers;

    [Header("Hidden blocks are revealed up to this distance.")]
    public int RevealDistance = 0;

    [Header("Temperature Source")]
    public int TemperatureSource = 0;

    public bool IsRotationFixture;

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

    [Header("SFX")]
    public string AddSFX = "";
    public string RemoveSFX = "";
    public string InteractSFX = "";

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

    #region ITagObject

    public string TagObjectID
    {
        get { return Key; }
    }

    public string TagObjectTooltipID
    {
        get { return TooltipID; }
    }

    [Header("Lifetime Minutes (-1 = disabled)")]
    public int MaxLifeTime = 0;

    [Header("Max amount to simulate for world gen")]
    public int MaxDefaultSimTime = 24 * 60 * 10;

    [Header("Simulation ID")]
    public string BlockimulationID = "";

    public void SetSimulationID(string simID)
    {
        BlockimulationID = simID;
    }

    public string SimulationID { get { return BlockimulationID; } }
    private GDESimulationData _simData;
    public GDESimulationData Simulation { get { return _simData; } }

    public SimTime MaxSimTime { get { return MaxDefaultSimTime; } }

    public const string BLOCK_STATE_CAN_FALL = "block_state_can_fall";

    [System.NonSerialized]
    private string[] _simStates = new string[]
    {
        BLOCK_STATE_CAN_FALL
    };

    public SimOptions[] StateOptions;

    [System.NonSerialized]
    public Dictionary<string, SimOptions> TagObjectsByOptionID = new Dictionary<string, SimOptions>();

    public string[] GetSimStates()
    {
        return _simStates;
    }

    public SimOptions[] GetOptions()
    {
        return StateOptions;
    }
    #endregion

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();


#if DEV_TESTING
        if (TemperatureSource > 0 && TagIDsHash.Add("tag_heat_source"))
        {
            TagIDs.Add("tag_heat_source");
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
