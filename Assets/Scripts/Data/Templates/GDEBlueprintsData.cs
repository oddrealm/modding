using System;
using System.Collections.Generic;
using UnityEngine;

public enum JobActionTypes
{
    COLLECT_AND_STORE = 0, // Pick up resource and bring to job location.
    COLLECT_STOCK = 1, // Pick up resource and fill location with resources until full.
    COLLECT = 2, // Pick up resource.
    WORK = 3, // Work on job progress.
    STORE = 4, // Bring resource to job.
    COLLECT_ALL = 5, // Any resources at job location will be carried by worker.
    STOCK = 6, // Fill location with resources until full.
    WORK_NO_PROGRESS = 7, // Do job until status is gone.
    // COLLECT_AND_SELL = 8, // Pick up resource and sell it.
    // SELL = 9, // Sell resource.
    GATHER_TARGET = 10, // Gather target worker.
    // COLLECT_AND_BUY = 11, // Pick up resource and buy it.
    // BUY = 12, // Buy resource.
}

public enum WorkPositionTypes
{
    CLOSEST = 0,
    CENTER = 1,
    CENTER_THEN_ADJACENT = 2,
}

[System.Serializable]
public struct JobActionRequirement
{
    public JobActionTypes ActionType; // This will dictate what the worker will do.
    public string ResourceID; // This is an optional resource required for the job.
    public WorkPositionTypes WorkPosition;
    public EntityAnimationTriggers AnimationTrigger;
    public EntityAnimationEvents AnimationHitEvent;
    public string AnimStartFX;
    public string AnimStartSFX;

    public readonly bool HasResourceRequirement { get { return !string.IsNullOrEmpty(ResourceID); } }

    public static JobActionRequirement NULL = new();
}

public enum JobPoolTypes
{
    SOURCE, // Get resources from the source (creator of the auto-job).
    WORLD, // Get resources from the world.
    WORKER_INVENTORY // Get resources from the worker's inventory.
}

public enum ResourceRestrictions
{
    ANY, // Anywhere in the world.
    OUTSIDE_LOCATION, // i.e., outside room
    INSIDE_LOCATION, // i.e., inside room
    CAN_STORE_AT_LOCATION, // i.e., put into container and/or stack
}

public enum LocationRestrictions
{
    ANY, // Anywhere in the world.
    EMPTY, // Location must have no resources in it.
}

[CreateAssetMenu(menuName = "ScriptableObjects/Blueprints")]
public class GDEBlueprintsData : Scriptable, IProgressionObject
{
    public int DesignationListOrder = -1;
    public bool SuppressDiscoveredNotif;
    [Header("Used to force jobs to be done. i.e., blueprint_eat_item = 99")]
    public int SkillPriorityOverride = -1;
    public int XP = 10;
    public int BaseSkillLevel = 1;
    public string CategoryID = "blueprint_category_jobs";
    public string[] ResearchKeys = Array.Empty<string>();
    public bool HideInResearchWindow = false;
    public bool Repeat = false;
    public bool CheckStateForTransition = false;
    public bool MarkItemsAtLocationClaimed = false;
    public bool MarkItemsAtLocationUnclaimed = false;
    public int SimTime = 0;
    [Header("Skill Failures:")]
    public bool FailJobOnSkillFailure = false;
    public string TargetAttributeForSkillFailCheck = "";
    public bool AddBlockSkillToJobSkill = false;
    [Header("Resource Permissions:")]
    public bool OnlyUseResourcesAtJobLocation = false;
    [Header("Location Permissions:")]
    public string LocationID = "";
    public WorkPositionTypes WorkPosition;
    public BlockPermissionTypes PermissionType = BlockPermissionTypes.NONE;
    public bool CanSitDuringJob = false;
    [Header("Worker Permissions:")]
    public string WorkerID0 = "";
    public string WorkerID1 = "";
    [Header("Target Permissions:")]
    public string TargetID0 = "";
    public string TargetID1 = "";
    [Header("Actions:")]
    public ResourceRestrictions ResourceRestriction = ResourceRestrictions.ANY;
    public LocationRestrictions LocationRestriction = LocationRestrictions.ANY;
    public JobActionRequirement[] JobActions = new JobActionRequirement[]
    {
        new()
        {
            ActionType = JobActionTypes.WORK
        }
    };
    [Header("Progress:")]
    public bool IsInstant = false;
    public string ProgressWorkerAttribute = "";
    public ProgressBarTypes ProgressBarType = ProgressBarTypes.CIRCLE;
    [Header("Progress FX X:")]
    public int ProgressFXMaxX = 0;
    public int ProgressFXMinX = 0;
    [Header("Progress FX Y:")]
    public int ProgressFXMaxY = 0;
    public int ProgressFXMinY = 0;
    [Header("On Finish:")]
    public BlockClear Clear;
    public int RevealDistance = 0;
    public int SpawnCount = 1;
    public string SpawnTagID;
    public string SpawnTagObjectID;
    public bool ConsumeWorker = false;
    [Header("Mass")]
    public bool ChangeFillMass = false;
    public int SetMass = 0;
    public string TriggerActivationID = "";
    public string ActiveWorkerStatus = "";
    public string OnJobStartActionID = "";
    public string OnJobQuitActionID = "";
    public string OnJobFinishActionID = "";
    public string OnItemConsumeActionID = "";
    public bool TargetIsConsumer = false;
    public bool DisableConsumeSpawns = false;
    public string SetTargetFactionOnComplete = "";
    public string SetTargetFactionOnSkillFailure = "";
    public BuffData[] OnProgressWorkerBuffs = Array.Empty<BuffData>();
    public BuffData[] OnProgressTargetBuffs = Array.Empty<BuffData>();
    public BuffData[] OnFinishWorkerBuffs = Array.Empty<BuffData>();
    public string[] OnFinishWorkerStatusAdds = Array.Empty<string>();
    public string[] OnFinishWorkerStatusRemoves = Array.Empty<string>();
    public BuffData[] OnFinishTargetBuffs = Array.Empty<BuffData>();
    public string[] OnFinishTargetStatusAdds = Array.Empty<string>();
    public string[] OnFinishTargetStatusRemoves = Array.Empty<string>();
    [Header("Hack")]
    public bool TryCatchFish = false;
    public bool TryHarvestFruit = false;
    [Header("Action Text:")]
    public bool ShowWorkOutputActionText = true;
    [Header("FX:")]
    public string OnFinishedFX = "";
    public string WorkFX = "fx_block_hit";
    [Header("SFX:")]
    public string WorkSFX = "sfx_hammer_wood";
    public string OnFinishedSFX = "";
    [Header("Tile Visuals:")]
    public bool ShowVisualsOnCursor = true;
    public List<string> ClearJobCursorVisuals = new() { "visuals_job_cursor_clear" };
    public List<string> PermittedCursorVisualsOverride = new();
    public List<string> ProhibitedCursorVisualsOverride = new();
    public List<string> Visuals = new();
    [Header("Auto-Job Settings")]
    public int MaxAutoJobQueueCount = 9;
    public bool DisposeIfRequirementsNotMet;

    public bool CanShowInProgressUI { get { return !SuppressDiscoveredNotif; } }
    public bool HasRotationFixture { get; private set; }
    public override string ObjectTypeDisplay { get { return "Blueprints"; } }
    public GDEBlueprintCategoryData CategoryData { get; private set; }
    public BlockLayers RequiredLayerPermissions { get; private set; } = BlockLayers.NONE;
    public bool HasWorkerRequirement { get { return !string.IsNullOrEmpty(WorkerID0); } }
    public bool HasTargetRequirement { get { return !string.IsNullOrEmpty(TargetID0); } }
    public bool HasLocationRequirement { get { return !string.IsNullOrEmpty(LocationID); } }
    public bool HasResourceRequirement { get { return RequiredItemIDs.Count > 0; } }
    public bool HasWorkerBuffOrStatus { get { return OnFinishWorkerBuffs.Length > 0 || OnFinishWorkerStatusAdds.Length > 0 || OnProgressWorkerBuffs.Length > 0; } }
    public bool HasTargetBuffOrStatus { get { return OnFinishTargetBuffs.Length > 0 || OnFinishTargetStatusAdds.Length > 0 || OnProgressTargetBuffs.Length > 0; } }
    public List<string> RequiredItemIDs { get; private set; } = new List<string>();
    public Dictionary<string, int> RequiredIDIndices { get; private set; } = new Dictionary<string, int>();
    public Dictionary<string, int> RequiredItemIDsAndCount { get; private set; } = new Dictionary<string, int>();

    public List<string> ResourcePermissionTagKeys { get; private set; } = new List<string>();
    public List<string> ResourcePermissionTagObjKeys { get; private set; } = new List<string>();
    public HashSet<string> ResourcePermissionTagKeysHash { get; private set; } = new HashSet<string>();
    public HashSet<string> ResourcePermissionTagObjKeysHash { get; private set; } = new HashSet<string>();
    public HashSet<string> AllRelevantResources { get; private set; } = new HashSet<string>();
    public HashSet<string> LocationPermissionTagKeys { get; private set; } = new HashSet<string>();
    public HashSet<string> WorkerPermissionTagKeys { get; private set; } = new HashSet<string>();
    public HashSet<string> TargetPermissionTagKeys { get; private set; } = new HashSet<string>();
    public bool HasOutput
    {
        get
        {
            if (!string.IsNullOrEmpty(SpawnTagID)) { return true; }
            if (!string.IsNullOrEmpty(SpawnTagObjectID)) { return true; }
            if (HasWorkerBuffOrStatus) { return true; }
            if (OnFinishWorkerStatusRemoves.Length > 0) { return true; }
            if (HasTargetBuffOrStatus) { return true; }
            if (OnFinishTargetStatusRemoves.Length > 0) { return true; }

            return false;
        }
    }
    public ITagObject SpawnTagObject { get; private set; }
    public List<string> UsedBy { get; private set; } = new();
    public List<string> PermittedSeasons { get; private set; } = new();
    public HashSet<string> PermittedSeasonsHash { get; private set; } = new();
    public bool CanBeRotated { get; private set; }

#if ODD_REALM_APP
    public override void SetOrderKey(string orderKey)
    {
        if (DesignationListOrder >= 0)
        {
            orderKey = $"{CategoryID}/{DesignationListOrder}/{BaseSkillLevel:D4}/{orderKey}";
        }
        else
        {
            orderKey = $"{CategoryID}/{99}/{BaseSkillLevel:D4}/{orderKey}";
        }

        base.SetOrderKey(orderKey);
    }

    public override void OnLoaded()
    {
        base.OnLoaded();

#if DEV_TESTING

        // if (Clear.Layer == BlockLayers.ITEMS)
        // {
        //     Debug.LogError($"Blueprint {Key} has Clear.Layer set to ITEMS which is not valid. Setting it to NONE.");
        //     Clear.Layer = BlockLayers.NONE;
        //     UnityEditor.EditorUtility.SetDirty(this);
        // }

        int initialBaseSkillLevel = BaseSkillLevel;
        const int dirtLevel = 5;
        const int clayLevel = 5;
        const int sandLevel = 5;
        const int fiberLevel = 5;
        const int stoneLevel = 10;
        const int sandstoneLevel = 10;
        const int glassLevel = 10;
        const int nyctiumLevel = 10;
        const int copperLevel = 10;
        const int tinLevel = 10;
        const int bronzeLevel = 10;
        const int ironLevel = 20;
        const int steelLevel = 25;
        const int voidLevel = 15;
        const int rutileLevel = 30;
        const int sableLevel = 30;

        if (Key.Contains("_dirt_"))
        {
            BaseSkillLevel = dirtLevel;
        }
        else if (Key.Contains("_clay_"))
        {
            BaseSkillLevel = clayLevel;
        }
        else if (Key.Contains("_sand_"))
        {
            BaseSkillLevel = sandLevel;
        }
        else if (Key.Contains("_fiber_"))
        {
            BaseSkillLevel = fiberLevel;
        }
        else if (Key.Contains("_stone_"))
        {
            BaseSkillLevel = stoneLevel;
        }
        else if (Key.Contains("_sandstone_"))
        {
            BaseSkillLevel = sandstoneLevel;
        }
        else if (Key.Contains("_glass_"))
        {
            BaseSkillLevel = glassLevel;
        }
        else if (Key.Contains("_nyctium_"))
        {
            BaseSkillLevel = nyctiumLevel;
        }
        else if (Key.Contains("_copper_"))
        {
            BaseSkillLevel = copperLevel;
        }
        else if (Key.Contains("_tin_"))
        {
            BaseSkillLevel = tinLevel;
        }
        else if (Key.Contains("_bronze_"))
        {
            BaseSkillLevel = bronzeLevel;
        }
        else if (Key.Contains("_iron_"))
        {
            BaseSkillLevel = ironLevel;
        }
        else if (Key.Contains("_void_"))
        {
            BaseSkillLevel = voidLevel;
        }
        else if (Key.Contains("_steel_"))
        {
            BaseSkillLevel = steelLevel;
        }
        else if (Key.Contains("_rutile_"))
        {
            BaseSkillLevel = rutileLevel;
        }
        else if (Key.Contains("_sable_"))
        {
            BaseSkillLevel = sableLevel;
        }
        else if (BaseSkillLevel > 50)
        {
            Debug.LogError($"BaseSkillLevel for {Key} is set to {BaseSkillLevel} which is above the max of 50. Setting it to 50.");
        }

        if (initialBaseSkillLevel != BaseSkillLevel)
        {
            Debug.Log($"Setting BaseSkillLevel for {Key} to {BaseSkillLevel} based on name.");
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

#endif

        EnsureTag("tag_blueprints");

        if (!string.IsNullOrEmpty(SpawnTagObjectID))
        {
            SpawnTagObject = DataManager.GetTagObject(SpawnTagObjectID);
        }

        PermittedSeasons.Clear();
        PermittedSeasonsHash.Clear();

        if (!string.IsNullOrEmpty(SpawnTagObjectID))
        {
            ITagObject tagObj = DataManager.GetTagObject(SpawnTagObjectID);

            if (tagObj is GDEBlockPlantsData plantData)
            {
                for (int i = 0; i < plantData.Seasons.Count; i++)
                {
                    if (PermittedSeasonsHash.Add(plantData.Seasons[i]))
                    {
                        PermittedSeasons.Add(plantData.Seasons[i]);
                    }
                }
            }
        }

        UsedBy.Clear();
        List<ITagObject> roomTemplates = DataManager.GetTagObjects<GDERoomTemplatesData>();

        for (int i = 0; i < roomTemplates.Count; i++)
        {
            GDERoomTemplatesData roomTemplate = DataManager.GetTagObject<GDERoomTemplatesData>(roomTemplates[i].Key);

            if (roomTemplate == null) { continue; }
            if (roomTemplate.IsNULL) { continue; }

            for (int j = 0; roomTemplate.DefaultAutoJobs != null && j < roomTemplate.DefaultAutoJobs.Count; j++)
            {
                if (roomTemplate.DefaultAutoJobs[j].BlueprintID == Key)
                {
                    UsedBy.Add(roomTemplate.Key);
                    break;
                }
            }
        }

        for (int i = 0; i < Visuals.Count; i++)
        {
            GDEBlockVisualsData visuals = DataManager.GetTagObject<GDEBlockVisualsData>(Visuals[i]);

            if ((visuals.RotationType & BlockRotationTypes.PLAYER_CAN_ROTATE) == 0) { continue; }

            CanBeRotated = true;
            break;
        }

        RebuildWorkerPermissions();
        RebuildTargetPermissions();
        RebuildLocationPermissions();
        RebuildResourcePermissions();
        CategoryData = DataManager.GetTagObject<GDEBlueprintCategoryData>(CategoryID);

#if DEV_TESTING
        for (int i = 0; i < ResearchKeys.Length; i++)
        {
            string researchKey = ResearchKeys[i];

            if (!string.IsNullOrEmpty(researchKey) && !DataManager.TagObjectExists(researchKey))
            {
                Debug.LogError("No ResearchKey data found for: " + researchKey + " blueprint: " + Key);
            }
        }

        if (!string.IsNullOrEmpty(SpawnTagObjectID) && DataManager.GetTagObject(SpawnTagObjectID).IsNULL)
        {
            Debug.LogError("No SpawnTagObjectID data found for: " + Key);
        }

        if (!string.IsNullOrEmpty(SpawnTagID) && DataManager.GetTagObjectsByTag(SpawnTagID).Count == 0)
        {
            Debug.LogError("No SpawnTagID data found for: " + Key);
        }
#endif

        RequiredLayerPermissions = BlockLayers.NONE;

        // Spawn tag layer permission
        if (!string.IsNullOrEmpty(SpawnTagID))
        {
            List<ITagObject> tagObjs = DataManager.GetTagObjectsByTag(SpawnTagID);

            for (int i = 0; i < tagObjs.Count; i++)
            {
                AddRequiredLayerPermission(tagObjs[i]);
            }
        }

        // Spawn tag object layer permission
        if (!string.IsNullOrEmpty(SpawnTagObjectID))
        {
            AddRequiredLayerPermission(DataManager.GetTagObject(SpawnTagObjectID));
        }

        DiscoveryDependenciesHash.Clear();
        DiscoveryDependenciesHash.UnionWith(DiscoveryDependencies);
        RequiredItemIDsAndCount.Clear();
        RequiredItemIDs.Clear();
        RequiredIDIndices.Clear();

        for (int i = 0; JobActions != null && i < JobActions.Length; i++)
        {
            if (!string.IsNullOrEmpty(JobActions[i].ResourceID))
            {
                if (DiscoveryDependenciesHash.Add(JobActions[i].ResourceID))
                {
                    DiscoveryDependencies.Add(JobActions[i].ResourceID);
                }

                if (RequiredItemIDsAndCount.ContainsKey(JobActions[i].ResourceID))
                {
                    RequiredItemIDsAndCount[JobActions[i].ResourceID]++;
                }
                else
                {
                    RequiredItemIDs.Add(JobActions[i].ResourceID);
                    RequiredIDIndices.Add(JobActions[i].ResourceID, RequiredItemIDs.Count - 1);
                    RequiredItemIDsAndCount.Add(JobActions[i].ResourceID, 1);
                }
            }
        }

        if (string.IsNullOrEmpty(WorkerID0))
        {
            Debug.LogError("Worker ID is not set for: " + Key);
        }

        if (!string.IsNullOrEmpty(SpawnTagObjectID))
        {
            if (DiscoveryDependenciesHash.Add(SpawnTagObjectID))
            {
                DiscoveryDependencies.Add(SpawnTagObjectID);
            }

        }

#if DEV_TESTING
        if (string.IsNullOrEmpty(WorkerID0))
        {
            Debug.LogError($"{Key} is missing core worker id!");
        }
#endif
    }

    private void RebuildWorkerPermissions()
    {
        WorkerPermissionTagKeys.Clear();
        AddWorkerTag(WorkerID0);
        AddWorkerTag(WorkerID1);
    }

    private void AddWorkerTag(string tagKey)
    {
        if (string.IsNullOrEmpty(tagKey)) { return; }

        ITagObject tagObj = DataManager.GetTagObject(tagKey);

        if (tagObj is GDETagsData)
        {
            WorkerPermissionTagKeys.Add(tagKey);
        }
        else
        {
            for (int i = 0; i < tagObj.TagCount; i++)
            {
                ITag tag = tagObj.GetTag(i);
                WorkerPermissionTagKeys.Add(tag.TagID);
            }
        }
    }

    private void RebuildTargetPermissions()
    {
        TargetPermissionTagKeys.Clear();
        AddTargetTag(TargetID0);
        AddTargetTag(TargetID1);
    }

    private void AddTargetTag(string tagKey)
    {
        if (string.IsNullOrEmpty(tagKey)) { return; }

        ITagObject tagObj = DataManager.GetTagObject(tagKey);

        if (tagObj is GDETagsData)
        {
            TargetPermissionTagKeys.Add(tagKey);
        }
        else
        {
            for (int i = 0; i < tagObj.TagCount; i++)
            {
                ITag tag = tagObj.GetTag(i);
                TargetPermissionTagKeys.Add(tag.TagID);
            }
        }
    }

    private void RebuildLocationPermissions()
    {
        LocationPermissionTagKeys.Clear();

        if (!string.IsNullOrEmpty(LocationID))
        {
            ITagObject tagObj = DataManager.GetTagObject(LocationID);

            if (tagObj is GDETagsData)
            {
                LocationPermissionTagKeys.Add(LocationID);
            }
            else
            {
                for (int i = 0; i < tagObj.TagCount; i++)
                {
                    ITag tag = tagObj.GetTag(i);
                    LocationPermissionTagKeys.Add(tag.TagID);
                }

                if (tagObj.TagCount == 0)
                {
                    Debug.LogError($"No tags found for: {tagObj.Key} in {Key}");
                }
            }
        }
    }

    private void RebuildResourcePermissions()
    {
        ResourcePermissionTagKeysHash.Clear();
        ResourcePermissionTagObjKeysHash.Clear();
        ResourcePermissionTagKeys.Clear();
        ResourcePermissionTagObjKeys.Clear();
        AllRelevantResources.Clear();

        for (int i = 0; i < JobActions.Length; i++)
        {
            if (string.IsNullOrEmpty(JobActions[i].ResourceID)) { continue; }

            ITagObject tagObj = DataManager.GetTagObject(JobActions[i].ResourceID);
            AddResourceTagObj(tagObj);
        }
    }

    private void AddResourceTagObj(ITagObject tagObj)
    {
        if (tagObj.IsNULL)
        {
            Debug.LogError($"Tag object is NULL for: {tagObj.Key} in {Key}");
            return;
        }

        if (tagObj is GDETagsData tagData)
        {
            if (tagObj.Key == "tag_item")
            {
                List<ITag> itemTagObjs = DataManager.GetTagsByGroup("item_type");

                for (int i = 0; i < itemTagObjs.Count; i++)
                {
                    AddResourceTag(itemTagObjs[i]);
                }

                return;
            }

            AddResourceTag(tagData);
            return;
        }

        if (!ResourcePermissionTagObjKeysHash.Add(tagObj.Key)) { return; }

        ResourcePermissionTagObjKeys.Add(tagObj.Key);
        AllRelevantResources.Add(tagObj.Key);
    }

    private void AddResourceTag(ITag tag)
    {
        if (!ResourcePermissionTagKeysHash.Add(tag.TagID)) { return; }

        ResourcePermissionTagKeys.Add(tag.TagID);

        List<ITagObject> tagObjs = DataManager.GetTagObjectsByTag(tag.TagID);

        for (int i = 0; i < tagObjs.Count; i++)
        {
            AllRelevantResources.Add(tagObjs[i].Key);
        }
    }

    private void AddRequiredLayerPermission(ITagObject tagObject)
    {
        if (tagObject is GDEBlocksData blockData)
        {
            RequiredLayerPermissions |= BlockLayers.BLOCK;
            HasRotationFixture |= blockData.IsRotationFixture;
        }
        else if (tagObject is GDEBlockPlantsData plantData)
        {
            RequiredLayerPermissions |= plantData.Layer;
        }
        else if (tagObject is GDEItemsData itemData)
        {
            RequiredLayerPermissions |= BlockLayers.ITEMS;
        }
        else if (tagObject is GDEBlockPlatformsData platformData)
        {
            RequiredLayerPermissions |= BlockLayers.PLATFORM;
        }
        else
        {
            // Error?
        }
    }
#endif
}
