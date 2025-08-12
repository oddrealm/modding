using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BlockFill")]
public class GDEBlockFillData : Scriptable, ISimulationData
{
    public BlockPermissionTypes Permissions = BlockPermissionTypes.NONE;
    public BlockPermissionTypes Prohibited = BlockPermissionTypes.NONE;
    public BlockPermissionTypes PermissionsAbove = BlockPermissionTypes.NONE;
    public BlockPermissionTypes ProhibitedAbove = BlockPermissionTypes.NONE;
    public int MovementCost = 0;
    public int FillPriority;
    public int MaxMass = 5;
    public bool FillNeighborBlocksOnNew = false;
    public bool IsObstruction = false;
    public bool IsVerticalAccess = false;
    public int EmptyThreshold = 10;
    public int FreezeThreshold = 0;
    public int MeltThreshold = 1;
    public int BurnThreshold = 100;
    public string EmptyFill = "block_fill_air";
    [Header("Visuals:")]
    public string SettledVisuals = "";
    public string UnsettledVisuals = "";
    public string FillFX = "";
    public string EmptyFX = "";
    [Header("Card Sprites:")]
    public string BackgroundIcon;
    public string MidgroundIcon;
    public string ForegroundIcon;
    [Header("Max amount to simulate for world gen")]
    public int MaxDefaultSimTime = 24 * 60 * 10;
    [Header("Simulation ID")]
    public string FillSimulationID = "";

    public override bool ShowMinimapCutoutColor { get { return true; } }
    public override bool ShowOnMinimap
    {
        get
        {
            return !string.IsNullOrEmpty(SettledVisuals);
        }
    }
    public override Color MinimapColor
    {
        get
        {
            if (string.IsNullOrEmpty(SettledVisuals)) { return Color.white; }

#if ODD_REALM_APP
            GDEBlockVisualsData blockVisuals = DataManager.GetTagObject<GDEBlockVisualsData>(SettledVisuals);

            return blockVisuals.MapColor;
#else
            return Color.white;
#endif
        }
    }
    public string SimulationID { get { return FillSimulationID; } }
    public GDESimulationData Simulation { get { return _simData; } }
    public SimTime MaxSimTime { get { return MaxDefaultSimTime; } }
    public float MaxMassReciprocal { get; private set; }
    public Sprite BackgroundSprite { get; private set; }
    public Sprite MidgroundSprite { get; private set; }
    public Sprite ForegroundSprite { get; private set; }
    public bool IsWater { get; private set; }
    public bool IsAir { get; private set; }
    public bool IsSolid { get; private set; }
    public bool IsFire { get; private set; }
    public GDEBlockFillData EmptyFillData { get; private set; }

    private GDESimulationData _simData;
    private string[] _simStates = new string[]
    {
        BLOCK_FILL_STATE_FULL,
        BLOCK_FILL_STATE_FILL_TARGET,
        BLOCK_FILL_STATE_EMPTY,
        BLOCK_FILL_STATE_FREEZE,
        BLOCK_FILL_STATE_MELT,
        BLOCK_FILL_STATE_BURN,
        BLOCK_FILL_CAN_MOVE_TO,
    };

    public const string BLOCK_FILL_STATE_FULL = "block_fill_state_full";
    public const string BLOCK_FILL_STATE_EMPTY = "block_fill_state_empty";
    public const string BLOCK_FILL_STATE_FILL_TARGET = "block_fill_state_fill_target";
    public const string BLOCK_FILL_STATE_FREEZE = "block_fill_state_freeze";
    public const string BLOCK_FILL_STATE_MELT = "block_fill_state_melt";
    public const string BLOCK_FILL_STATE_BURN = "block_fill_state_burn";
    public const string BLOCK_FILL_CAN_MOVE_TO = "block_fill_can_move_to";

    public void SetSimulationID(string simID)
    {
        FillSimulationID = simID;
    }

    public string[] GetSimStates()
    {
        return _simStates;
    }

    public SimOptions[] GetOptions()
    {
        return null;
    }

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        EmptyFillData = DataManager.GetTagObject<GDEBlockFillData>(EmptyFill);
        IsWater = Key == "block_fill_water";
        IsAir = Key == "block_fill_air";
        IsSolid = Key == "block_fill_solid";
        IsFire = Key == "block_fill_fire";

        if (!string.IsNullOrEmpty(SimulationID))
        {
            _simData = DataManager.GetTagObject<GDESimulationData>(SimulationID);
        }

        MaxMassReciprocal = 1f / (float)MaxMass;
        BackgroundSprite = string.IsNullOrEmpty(BackgroundIcon) ? null : GlobalSettingsManager.GetIcon(BackgroundIcon);
        MidgroundSprite = string.IsNullOrEmpty(MidgroundIcon) ? null : GlobalSettingsManager.GetIcon(MidgroundIcon);
        ForegroundSprite = string.IsNullOrEmpty(ForegroundIcon) ? null : GlobalSettingsManager.GetIcon(ForegroundIcon);
        base.OnLoaded();
    }
#endif
}
