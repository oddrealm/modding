using System;
using System.Collections.Generic;
using UnityEngine;

public enum JobActionTypes
{
    COLLECT_AND_STORE = 0, // Pick up resource and bring to job location.
    COLLECT_AND_STOCK = 1, // Pick up resource and fill location with resources until full.
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

    public bool HasResourceRequirement { get { return !string.IsNullOrEmpty(ResourceID); } }

    public static JobActionRequirement NULL = new JobActionRequirement();
}

public enum JobPoolTypes
{
    SOURCE, // Get resources from the source (creator of the auto-job).
    WORLD, // Get resources from the world.
    WORKER_INVENTORY // Get resources from the worker's inventory.
}

[CreateAssetMenu(menuName = "ScriptableObjects/Blueprints")]
public class GDEBlueprintsData : Scriptable, IProgressionObject
{
    [Header("Used to force jobs to be done. i.e., blueprint_eat_item = 99")]
    public int SkillPriorityOverride = -1;
    public int XP = 10;
    public bool AwardXPForEachAction = false;
    public string CategoryID = "blueprint_category_jobs";
    public int DesignationListOrder = -1;
    public string ResearchKey = "";
    public bool Repeat = false;
    public bool CheckStateForTransition = false;
    public bool MarkItemsAtLocationClaimed = false;
    public bool MarkItemsAtLocationUnclaimed = false;
    public int SimTime = 0;
    public bool AddBlockSkillToJobSkill = false;
    [Header("Resource Permissions:")]
    public bool OnlyUseResourcesAtJobLocation = false;
    public bool ShowItemTypeResourcePermissions = false;
    [Header("Location Permissions:")]
    public string LocationID = "";
    public WorkPositionTypes WorkPosition;
    public BlockPermissionTypes PermissionType = BlockPermissionTypes.NONE;
    public bool CanSitDuringJob = false;
    [Header("Worker Permissions:")]
    public string WorkerID = "";
    [Header("Target Permissions:")]
    public string TargetID = "";
    [Header("Actions:")]
    public JobActionRequirement[] JobActions = new JobActionRequirement[]
    {
        new JobActionRequirement()
        {
            ActionType = JobActionTypes.WORK
        }
    };
    [Header("Progress:")]
    public string ProgressWorkerAttribute = "";
    public int AddedProgressPerAction = 0;
    public int ProgressMax = 100;
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
    [Header("Mass")]
    public bool ChangeFillMass = false;
    public int SetMass = 0;
    public string TriggerActivationID = "";
    public string ActiveWorkerStatus = "";
    public string OnJobStartActionID = "";
    public string OnJobQuitActionID = "";
    public string OnJobFinishActionID = "";
    public string OnItemConsumeActionID = "";
    public bool DisableConsumeSpawns = false;
    public string SetTargetFactionTo = "";
    public BuffData[] OnFinishWorkerBuffs = Array.Empty<BuffData>();
    public string[] OnFinishWorkerStatusAdds = Array.Empty<string>();
    public string[] OnFinishWorkerStatusRemoves = Array.Empty<string>();
    public BuffData[] OnFinishTargetBuffs = Array.Empty<BuffData>();
    public string[] OnFinishTargetStatusAdds = Array.Empty<string>();
    public string[] OnFinishTargetStatusRemoves = Array.Empty<string>();
    [Header("Hack")]
    public bool TryCatchFish = false;
    public bool TryHarvestFruit = false;
    public bool CanSelectTarget = false;
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
    public List<string> ClearJobCursorVisuals = new List<string>() { "visuals_job_cursor_clear" };
    public List<string> PermittedCursorVisualsOverride = new List<string>();
    public List<string> ProhibitedCursorVisualsOverride = new List<string>();
    public List<string> Visuals = new List<string>();
    public bool ShowVisualsIfStarted = true;
    [Header("Auto-Job Settings")]
    public int MaxAutoJobQueueCount = 9;
    public bool DisposeIfRequirementsNotMet;
    public bool DisposeIfNoSourceEntity = false;
    public int AutoJobOnCreateCooldown = 0;
    public int AutoJobOnFreedCooldown = 0;

    public bool CanShowInProgressUI { get { return true; } }
    public bool HasRotationFixture { get; private set; }
    public override string ObjectTypeDisplay { get { return "Blueprints"; } }
    public GDEBlueprintCategoryData CategoryData { get; private set; }
    public BlockLayers RequiredLayerPermissions { get; private set; } = BlockLayers.NONE;
    public bool HasWorkerBuffOrStatus { get { return OnFinishWorkerBuffs.Length > 0 || OnFinishWorkerStatusAdds.Length > 0; } }
    public bool HasTargetBuffOrStatus { get { return OnFinishTargetBuffs.Length > 0 || OnFinishTargetStatusAdds.Length > 0; } }
    public List<string> RequiredItemIDs { get; private set; } = new List<string>();
    public Dictionary<string, int> RequiredItemIDsAndCount { get; private set; } = new Dictionary<string, int>();
    public HashSet<string> ResourcesPermissionTagKeys { get; private set; } = new HashSet<string>();
    public HashSet<string> ResourcesPermissionTagObjKeys { get; private set; } = new HashSet<string>();
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
    public bool IsStockpileJob { get; private set; }
    public bool IsTradeJob { get; private set; }
    public ITagObject SpawnTagObject { get; private set; }

#if ODD_REALM_APP
    public override void SetOrderKey(string orderKey)
    {
        orderKey = CategoryID + "/" + orderKey;
        base.SetOrderKey(orderKey);
    }

    public override void OnLoaded()
    {
        base.OnLoaded();

        if (!string.IsNullOrEmpty(SpawnTagObjectID))
        {
            SpawnTagObject = DataManager.GetTagObject(SpawnTagObjectID);
        }

        IsTradeJob = Key == "blueprint_buy_item";
        IsStockpileJob = Key == "blueprint_stock_room";
        RebuildWorkerPermissions();
        RebuildTargetPermissions();
        RebuildLocationPermissions();
        RebuildResourcePermissions();
        CategoryData = DataManager.GetTagObject<GDEBlueprintCategoryData>(CategoryID);

#if DEV_TESTING
        if (!string.IsNullOrEmpty(ResearchKey) && !DataManager.TagObjectExists(ResearchKey))
        {
            Debug.LogError("No ResearchKey data found for: " + ResearchKey + " blueprint: " + Key);
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
                    RequiredItemIDsAndCount.Add(JobActions[i].ResourceID, 1);
                }
            }
        }

        if (string.IsNullOrEmpty(WorkerID))
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
        if (string.IsNullOrEmpty(WorkerID))
        {
            Debug.LogError($"{Key} is missing core worker id!");
        }
#endif
    }

    private void RebuildWorkerPermissions()
    {
        WorkerPermissionTagKeys.Clear();

        if (string.IsNullOrEmpty(WorkerID)) { return; }

        ITagObject tagObj = DataManager.GetTagObject(WorkerID);

        if (tagObj is GDETagsData)
        {
            WorkerPermissionTagKeys.Add(WorkerID);
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

        if (string.IsNullOrEmpty(TargetID)) { return; }

        ITagObject tagObj = DataManager.GetTagObject(TargetID);

        if (tagObj is GDETagsData)
        {
            TargetPermissionTagKeys.Add(TargetID);
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
        ResourcesPermissionTagKeys.Clear();
        ResourcesPermissionTagObjKeys.Clear();

        if (ShowItemTypeResourcePermissions)
        {
            List<ITag> itemTypeTags = DataManager.GetTagsByGroup("item_type");

            for (int i = 0; i < itemTypeTags.Count; i++)
            {
                AddResourceTag(itemTypeTags[i]);
            }
        }
        else
        {
            for (int i = 0; i < JobActions.Length; i++)
            {
                if (string.IsNullOrEmpty(JobActions[i].ResourceID)) { continue; }

                ITagObject tagObj = DataManager.GetTagObject(JobActions[i].ResourceID);
                AddResourceTagObj(tagObj);
            }
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
            AddResourceTag(tagData);
            return;
        }

        ResourcesPermissionTagObjKeys.Add(tagObj.Key);
    }

    private void AddResourceTag(ITag tag)
    {
        ResourcesPermissionTagKeys.Add(tag.TagID);
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
