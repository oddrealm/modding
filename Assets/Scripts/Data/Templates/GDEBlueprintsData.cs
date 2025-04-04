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
    COLLECT_AND_SELL = 8, // Pick up resource and sell it.
    SELL = 9, // Sell resource.
    GATHER_TARGET = 10, // Gather target worker.
    COLLECT_AND_BUY = 11, // Pick up resource and buy it.
    BUY = 12, // Buy resource.
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
    public int MaxWorkers;
    public string WorkerID; // This is who can do the job.
    public string TargetID;
    public string ResourceID; // This is an optional resource required for the job.
    public string OnJobFinishActionID; // This is the action id to call when the job is successfully finished.
    public WorkPositionTypes WorkPosition;
    public EntityAnimationTriggers AnimationTrigger;
    public EntityAnimationEvents AnimationHitEvent;
    public string AnimStartFX;
    public string AnimStartSFX;

    public bool HasResourceRequirement { get { return !string.IsNullOrEmpty(ResourceID); } }
    public bool IsNULL { get { return string.IsNullOrEmpty(WorkerID); } }

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
    public string WorkerID = "";
    [Header("Used to force jobs to be done. i.e., blueprint_eat_item = 99")]
    public int SkillPriorityOverride = -1;
    public int XP = 10;
    public bool AwardXPForEachAction = false;
    public string CategoryID = "blueprint_category_jobs";
    public string ResearchKey = "";
    public bool Repeat = false;
    public bool CheckStateForTransition = false;
    public bool MarkItemsAtLocationClaimed = false;
    public bool MarkItemsAtLocationUnclaimed = false;
    public int SimTime = 0;
    public bool AddBlockSkillToJobSkill = false;
    [Header("Location Permissions:")]
    public string LocationID = "";
    public WorkPositionTypes WorkPosition;
    public BlockPermissionTypes PermissionType = BlockPermissionTypes.NONE;
    public List<string> LocationPermissions = new List<string>()
    {
    };
    [Header("Worker Permissions:")]
    public List<string> WorkerPermissions = new List<string>()
    {
        "faction_player",
        "faction_captured"
    };
    public List<string> WorkerPermissionsToLock = new List<string>();
    [Header("Target Permissions:")]
    public List<string> TargetPermissions = new List<string>()
    {
        "faction_player",
        "faction_captured"
    };
    public List<string> TargetPermissionsToLock = new List<string>();
    [Header("Resource Permissions:")]
    public bool OnlyUseResourcesAtJobLocation = false;
    public JobPoolTypes ResourcePool = JobPoolTypes.WORLD;
    public bool ShowItemTypeResourcePermissions = false;
    public List<string> ResourcePermissionsToLock = new List<string>();
    [Header("Actions:")]
    public int MaxWorkers = 0;
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
    public bool InvertProgressDisplay = false;
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
    public string OnFinishedResourcesActionID = "";
    public string SetTargetFactionTo = "";
    public BuffData[] OnFinishWorkerBuffs = new BuffData[] { };
    public string[] OnFinishWorkerStatusAdds = new string[] { };
    public string[] OnFinishWorkerStatusRemoves = new string[] { };
    public BuffData[] OnFinishTargetBuffs = new BuffData[] { };
    public string[] OnFinishTargetStatusAdds = new string[] { };
    public string[] OnFinishTargetStatusRemoves = new string[] { };
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
    public bool DisposeIfNoSource = false;
    public int AutoJobOnCreateCooldown = 0;
    public int AutoJobOnFreedCooldown = 0;

    public bool CanShowInProgressUI { get { return true; } }
    public bool HasRotationFixture { get; private set; }
    public override string ObjectTypeDisplay { get { return "Blueprints"; } }
    public GDEBlueprintCategoryData CategoryData { get; private set; }
    public BlockLayers RequiredLayerPermissions { get; private set; } = BlockLayers.NONE;
    public bool HasWorkerBuffOrStatus { get { return OnFinishWorkerBuffs.Length > 0 || OnFinishWorkerStatusAdds.Length > 0; } }
    public bool HasTargetBuffOrStatus { get { return OnFinishTargetBuffs.Length > 0 || OnFinishTargetStatusAdds.Length > 0; } }
    public List<string> JobActionResourceIDs { get; private set; } = new List<string>();
    public Dictionary<string, int> JobActionResourceIDsAndCount { get; private set; } = new Dictionary<string, int>();
    public List<string> JobActionTargetIDs { get; private set; } = new List<string>();
    public Dictionary<string, int> JobActionTargetIDsAndCount { get; private set; } = new Dictionary<string, int>();
    public HashSet<string> ResourcePermissionsToLockHash { get; private set; } = new HashSet<string>();
    public List<string> ResourcesPermissionKeys { get; private set; } = new List<string>();
    public HashSet<string> ResourcesPermissionKeysHash { get; private set; } = new HashSet<string>();
    public HashSet<string> TargetPermissionsToLockHash { get; private set; } = new HashSet<string>();
    public HashSet<string> WorkerPermissionsToLockHash { get; private set; } = new HashSet<string>();

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

#if ODD_REALM_APP
    public override void SetOrderKey(string orderKey)
    {
        orderKey = CategoryID + "/" + orderKey;
        base.SetOrderKey(orderKey);
    }

    public override void OnLoaded()
    {
        base.OnLoaded();

        if (!string.IsNullOrEmpty(LocationID) && !LocationPermissions.Contains(LocationID))
        {
            LocationPermissions.Add(LocationID);
        }

        ResourcePermissionsToLockHash.Clear();

        if (ResourcePermissionsToLock.Count > 0)
        {
            ResourcePermissionsToLockHash.Clear();
            ResourcePermissionsToLockHash.UnionWith(ResourcePermissionsToLock);
        }

        RebuildResourcePermissions();

        WorkerPermissionsToLockHash.Clear();

        if (WorkerPermissionsToLock.Count > 0)
        {
            WorkerPermissionsToLockHash.Clear();
            WorkerPermissionsToLockHash.UnionWith(WorkerPermissionsToLock);
        }

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

        JobActionResourceIDsAndCount.Clear();
        JobActionResourceIDs.Clear();

        JobActionTargetIDsAndCount.Clear();
        JobActionTargetIDs.Clear();

        bool test = false;
        for (int i = 0; JobActions != null && i < JobActions.Length; i++)
        {
#if DEV_TESTING
            //if (JobActions[i].ActionType == JobActionTypes.WORK)
            //{
            //	WorkPosition = JobActions[i].WorkPosition;
            //	UnityEditor.EditorUtility.SetDirty(this);
            //         }
            if (!test)
            {
            }
#endif

//#if UNITY_EDITOR
//            if (!string.IsNullOrEmpty(JobActions[i].OnJobFinishActionID))
//            {
//                Debug.LogError(Key + " has OnJobFinishActionID set: " + JobActions[i].OnJobFinishActionID);
//            }

//            if (!string.IsNullOrEmpty(OnFinishedResourcesActionID))
//            {
//                Debug.Log(Key + " has OnFinishedResourcesActionID set: " + OnFinishedResourcesActionID);
//            }
//#endif



            if (!string.IsNullOrEmpty(JobActions[i].ResourceID))
            {
                if (DiscoveryDependenciesHash.Add(JobActions[i].ResourceID))
                {
                    DiscoveryDependencies.Add(JobActions[i].ResourceID);
                }

                // Track resource requirements.
                if (JobActionResourceIDsAndCount.ContainsKey(JobActions[i].ResourceID))
                {
                    JobActionResourceIDsAndCount[JobActions[i].ResourceID]++;
                }
                else
                {
                    JobActionResourceIDs.Add(JobActions[i].ResourceID);
                    JobActionResourceIDsAndCount.Add(JobActions[i].ResourceID, 1);
                }
            }

            if (!string.IsNullOrEmpty(JobActions[i].WorkerID))
            {
                if (DiscoveryDependenciesHash.Add(JobActions[i].WorkerID))
                {
                    DiscoveryDependencies.Add(JobActions[i].WorkerID);
                }
            }
            else
            {
                Debug.LogError("Worker ID is not set for: " + Key);
            }

            if (!string.IsNullOrEmpty(JobActions[i].TargetID))
            {
                if (DiscoveryDependenciesHash.Add(JobActions[i].TargetID))
                {
                    DiscoveryDependencies.Add(JobActions[i].TargetID);
                }

                // Track Target requirements.
                if (JobActionTargetIDsAndCount.ContainsKey(JobActions[i].TargetID))
                {
                    JobActionTargetIDsAndCount[JobActions[i].TargetID]++;
                }
                else
                {
                    JobActionTargetIDs.Add(JobActions[i].TargetID);
                    JobActionTargetIDsAndCount.Add(JobActions[i].TargetID, 1);
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

    private void RebuildResourcePermissions()
    {
        ResourcesPermissionKeys.Clear();
        ResourcesPermissionKeysHash.Clear();

        if (ShowItemTypeResourcePermissions)
        {
            List<ITag> itemTypeTags = DataManager.GetTagsByGroup("item_type");

            for (int i = 0; i < itemTypeTags.Count; i++)
            {
                ITagObject tagObj = DataManager.GetTagObject(itemTypeTags[i].TagID);

                if (ResourcesPermissionKeysHash.Add(tagObj.Key))
                {
                    ResourcesPermissionKeys.Add(tagObj.Key);
                }
            }
        }
        else
        {
            for (int i = 0; i < JobActions.Length; i++)
            {
                if (string.IsNullOrEmpty(JobActions[i].ResourceID)) { continue; }

                ITagObject tagObj = DataManager.GetTagObject(JobActions[i].ResourceID);
                
                if (ResourcesPermissionKeysHash.Add(tagObj.Key))
                {
                    ResourcesPermissionKeys.Add(tagObj.Key);
                }
            }
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
