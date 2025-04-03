using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlantAction
{
    public string Comment;

    [Header("Used to activate from external sources (Optional). i.e., on watered 'tag_can_be_watered' plant action.")]
    public string ActionID;

    public BuffData Buff;
}

[CreateAssetMenu(menuName = "OddRealm/Block/BlockPlants", order = 22)]
public class GDEBlockPlantsData : Scriptable, ISimulationData
{
    [Header("Lifetime Minutes (-1 = disabled)")]
    public int MaxLifeTime = 0;
    [System.NonSerialized]
    public int MatureTime = 0;
    [System.NonSerialized]
    public int MatureTimeInFarm = 0;
    [Header("Reproduce Minutes")]
    public int ReproduceTimeInterval = 60 * 12;
    [Header("Max Natural Reproduce Count - only reproduce when plant count < this")]
    [Header("(World size modifies this value by (blocks per layer / 16384)")]
    public int MaxNaturalSpawnCount = 0;
    public int MaxNaturalReproduceCount = 0;
    public string ReproductionPlant = "";
    [Header("Max amount to simulate for world gen")]
    public int MaxDefaultSimTime = 24 * 60 * 10;
    [Header("Mature Time (% of Lifetime)")]
    [Range(0f, 1f)]
    public float MatureThreshold = 0.5f;
    [Header("Mature Tag (i.e., tag_mature_tree")]
    public string MatureTag = "tag_mature_standing";
    [System.NonSerialized]
    public ITagObject MatureTagObj;
    public string MatureInlineIcon = "<sprite=1183>";

    [Header("If the plant gets bigger than this size, it grows much less.")]
    public int LimitGroupSizeThreshold = 2500;

    [Header("Item Spawns")]
    public string RemoveItemDrops = "";
    public string MatureItemDrops = "";

    [Header("Tag Obj Spawns")]
    public string MatureTagObjectSpawn = "";
    public bool AutoClearOnMature = false;
    public bool SpawnItemsOnAutoClear = false;

    [Header("Visual Stages")]
    public int GrowthStages = 0;
    [Header("Health (-1 = disabled)")]
    public int HealthMax = 100;

    [Header("Hydration (-1 = disabled)")]
    public int HydrationMax = 100;
    public int HydrationDecay = -1;
    [Header("Dehydrated Tag (i.e., tag_can_be_watered")]
    public string DehydratedTag = "tag_can_be_watered";
    public string HydrationTooltip = "tooltip_plant_hydration";
    public string HydrationJob = "blueprint_water_plant";
    [System.NonSerialized]
    public ITagObject DehydratedTagObj;

    [Header("Skylight 0 to 7 (-1 = disabled)")]
    public int MinLight = 7;
    [Header("")]
    public string FillRequirement = "block_fill_air";
    public bool NeedsFertileSoil = true;
    [Header("Temperature Celsius -99 to 99 (-1 = disabled)")]
    public int MaxTemp = 0;
    public int MinTemp = 0;

    [Header("Simulation ID")]
    public string PlantSimulationID = "";

    [Header("Each tile has its own fauna rarity - if true, use this")]
    public bool UseLocalFaunaReproduceMod = true;

    [Header("Seasons (Empty = any season permitted)")]
    public List<string> Seasons = new List<string>();
    [System.NonSerialized]
    public HashSet<string> SeasonsHash = new HashSet<string>();

    [Header("Pathing")]
    public bool IsObstruction = false;
    public BlockDirectionFlags PermittedPaths = BlockDirectionFlags.ALL;
    public bool NeedsSupport = true;

    [Header("Fruit")]
    //public bool CanBeHarvestedForFruit = false;
    public string FruitingTag = "tag_mature_fruit";
    public ITagObject FruitingTagObj { get; private set; }
    public string FruitItemDrops = "";
    public int FruitMaxCount = 1;
    public int FruitMinInterval = 64;
    public int FruitMaxInterval = 64;
    public List<string> FruitSeasons;
    public string FruitVisuals;
    public bool CanGrowFruit { get { return !string.IsNullOrEmpty(FruitItemDrops); } }

    [Header("Layers")]
    public int LayerPriority;
    public BlockLayers Layer;
    public BlockLayers ProhibitedLayers;

    [Header("Permissions")]
    public BlockPermissionTypes Permissions = BlockPermissionTypes.NONE;
    public BlockPermissionTypes Prohibited = BlockPermissionTypes.NONE;
    public BlockPermissionTypes MaturePermissions = BlockPermissionTypes.NONE;
    public BlockPermissionTypes MatureProhibitions = BlockPermissionTypes.NONE;
    public BlockPlantTypes PlantType = BlockPlantTypes.NONE;
    public BlockPlantTypes PlantPermission = BlockPlantTypes.NONE;
    public bool CanHaveGroup = false;

    [Header("Visuals")]
    public List<string> Visuals = new List<string>();


    [Header("FX")]
    public string RemoveFX = "fx_remove_plant";

    [Header("Actions")]
    public PlantAction[] Actions = new PlantAction[0];

    private bool _isNull;
    public override bool IsNULL { get { return _isNull; } }


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

    public override Color MinimapColor
    {
        get
        {
            if (Visuals.Count == 0) { return Color.white; }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return DataUtility.ImportDataSingle<GDEBlockVisualsData>(Visuals[0]).MinimapColor;
            }
#endif

#if ODD_REALM_APP
            GDEBlockVisualsData blockVisuals = DataManager.GetTagObject<GDEBlockVisualsData>(Visuals[0]);

            return blockVisuals.MapColor;
#else
            return Color.white;
#endif
        }
    }

    public void SetSimulationID(string simID)
    {
        PlantSimulationID = simID;
    }

    public string SimulationID { get { return PlantSimulationID; } }
    private GDESimulationData _simData;
    public GDESimulationData Simulation { get { return _simData; } }

    public SimTime MaxSimTime { get { return MaxDefaultSimTime; } }

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        if (!IsObstruction && PermittedPaths == BlockDirectionFlags.NONE)
        {
            Debug.LogError($"BlockPlantsData {Key} has no pathing permissions set. Defaulting to all paths.");
        }

        if (MaxLifeTime > 0)
        {
            MatureTime = MaxLifeTime / 2;
            MatureTimeInFarm = MaxLifeTime / 3;
        }

        _isNull = Key == "plant_none";

        //if (_isNull) { return; }
        if (!string.IsNullOrEmpty(SimulationID))
        {
            _simData = DataManager.GetTagObject<GDESimulationData>(SimulationID);
        }

        SeasonsHash.Clear();

        for (int i = 0; i < Seasons.Count; i++)
        {
            SeasonsHash.Add(Seasons[i]);
        }

        TagObjectsByOptionID.Clear();

        for (int i = 0; StateOptions != null && i < StateOptions.Length; i++)
        {
            TagObjectsByOptionID.Add(StateOptions[i].OptionID, StateOptions[i]);
            StateOptions[i].TagObj = DataManager.GetTagObject(StateOptions[i].TagObjectID);
        }

        MatureTagObj = DataManager.GetTagObject(MatureTag);
        FruitingTagObj = DataManager.GetTagObject(FruitingTag);
        DehydratedTagObj = DataManager.GetTagObject(DehydratedTag);

        //if (HydrationMax >= 100)
        //{
        //	HydrationMax = 500;
        //	UnityEditor.EditorUtility.SetDirty(this);
        //}

        base.OnLoaded();
    }
#endif

    public const string PLANT_STATE_MATURE = "plant_state_mature";
    public const string PLANT_STATE_EXPIRED = "plant_state_expired";
    public const string PLANT_STATE_CAN_REPRODUCE = "plant_state_can_reproduce";
    public const string PLANT_STATE_CAN_MOVE_TO_TARGET = "plant_state_can_move_to_target";
    public const string PLANT_STATE_CAN_ADD_TO_TARGET = "plant_state_can_add_to_target";
    public const string PLANT_STATE_IS_PERMITTED = "plant_is_permitted";
    public const string PLANT_STATE_SPAWN = "plant_spawn";
    public const string PLANT_STATE_HAS_TAG_OBJECT_ID = "plant_has_tag_object_ID";

    [System.NonSerialized]
    private string[] _simStates = new string[]
    {
        PLANT_STATE_MATURE,
        PLANT_STATE_EXPIRED,
        PLANT_STATE_CAN_REPRODUCE,
        PLANT_STATE_CAN_MOVE_TO_TARGET,
        PLANT_STATE_CAN_ADD_TO_TARGET,
        PLANT_STATE_IS_PERMITTED,
        PLANT_STATE_SPAWN,
        PLANT_STATE_HAS_TAG_OBJECT_ID
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
}
