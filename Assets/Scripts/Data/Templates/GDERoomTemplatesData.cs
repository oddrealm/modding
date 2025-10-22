using Assets.GameData;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomTemplates")]
public class GDERoomTemplatesData : Scriptable
{
    [System.Serializable]
    public struct RoomAutoJob
    {
        public string BlueprintID;
        public AutoJobSettings Settings;
        public string[] ProhibitedResources;
        public string[] PermittedResources;
        public string[] ProhibitedLocations;
        public string[] PermittedLocations;
    }

    [System.Serializable]
    public struct RoomQuality
    {
        public int level;
        public string jobFinishWorkerAction;
        public string jobFinishItemAction;
    }

    [Header("Category")]
    public string CategoryID = "room_category_homes";

    [Header("Research")]
    public string ResearchKey = "";

    [Header("Max rooms that can be placed in a settlement.")]
    public int MaxRoomAmount = -1;

    [Header("Occupants")]
    public List<string> OccupantGroups = new List<string>();

    [Header("Visuals")]
    public List<string> Visuals = new List<string>();

    [Header("Jobs")]
    public List<RoomAutoJob> DefaultAutoJobs = new List<RoomAutoJob>();

    [Header("Quality")]
    public List<RoomQuality> RoomQualities = new List<RoomQuality>();

    [Header("Room Management")]
    public bool ShowProduction = true;
    public bool ShowStockpile = true;
    public bool ShowOccupants = true;
    public bool ShowMarket = true;

    public override string ObjectTypeDisplay { get { return "Rooms"; } }
    public List<string> CommonProps { get; private set; } = new List<string>();
    public HashSet<string> CommonPropsSet { get; private set; } = new HashSet<string>();
    public HashSet<string> DefaultAutoJobsHash { get; private set; } = new HashSet<string>();
    public Dictionary<string, Dictionary<string, bool>> DefaultProhibitedLocations { get; private set; } = new Dictionary<string, Dictionary<string, bool>>();
    public Dictionary<string, Dictionary<string, bool>> DefaultProhibitedResources { get; private set; } = new Dictionary<string, Dictionary<string, bool>>();
    public Dictionary<int, List<RoomQuality>> RoomQualityByLevel { get; private set; } = new Dictionary<int, List<RoomQuality>>();
    public int MaxQuality { get; private set; }

    public override bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking()
        {
            TagID = "",
            TagObjectID = Key,
            HideIfZero = true,
            StartDisabled = false,
            TrackingType = TrackingTypes.ROOM
        };

        return true;
    }

    public bool TryGetRoomQualitiesByLevel(int level, out List<RoomQuality> qualityList)
    {
        if (RoomQualityByLevel.TryGetValue(level, out qualityList))
        {
            return true;
        }

        return false;
    }

#if ODD_REALM_APP
    public bool IsLocationProhibitedByDefault(string blueprintID, string locationID)
    {
        if (string.IsNullOrEmpty(blueprintID) || string.IsNullOrEmpty(locationID) || DefaultProhibitedLocations.Count == 0)
        {
            return false;
        }

        if (DefaultProhibitedLocations.TryGetValue(blueprintID, out var locationPermissions) &&
            locationPermissions.TryGetValue(locationID, out var isProhibited))
        {
            return isProhibited;
        }

        return false;
    }

    public bool IsResourceProhibitedByDefault(string blueprintID, string resourceID)
    {
        if (string.IsNullOrEmpty(blueprintID) || string.IsNullOrEmpty(resourceID) || DefaultProhibitedResources.Count == 0)
        {
            return false;
        }

        if (DefaultProhibitedResources.TryGetValue(blueprintID, out var resourcePermissions) &&
            resourcePermissions.TryGetValue(resourceID, out var isProhibited))
        {
            return isProhibited;
        }

        return false;
    }

    public override void OnLoaded()
    {
        EnsureTag("tag_rooms");
        RoomQualityByLevel.Clear();
        MaxQuality = 0;

        for (int i = 0; i < RoomQualities.Count; i++)
        {
            RoomQuality quality = RoomQualities[i];

            if (!RoomQualityByLevel.TryGetValue(quality.level, out var qualityList))
            {
                qualityList = new List<RoomQuality>();
                RoomQualityByLevel[quality.level] = qualityList;
            }

            qualityList.Add(quality);
            MaxQuality = Mathf.Max(MaxQuality, quality.level);
        }

        DefaultAutoJobsHash.Clear();

        for (int i = 0; DefaultAutoJobs != null && i < DefaultAutoJobs.Count; i++)
        {
            RoomAutoJob autoJob = DefaultAutoJobs[i];

            if (!DefaultAutoJobsHash.Add(autoJob.BlueprintID))
            {
                Debug.LogError($"Duplicate auto job blueprint ID found: {autoJob.BlueprintID} in {Key}");
            }
        }

        CommonProps.Clear();
        CommonPropsSet.Clear();
        DefaultProhibitedLocations.Clear();
        DefaultProhibitedResources.Clear();

        for (int i = 0; DefaultAutoJobs != null && i < DefaultAutoJobs.Count; i++)
        {
            RoomAutoJob autoJob = DefaultAutoJobs[i];
            GDEBlueprintsData blueprint = DataManager.GetTagObject<GDEBlueprintsData>(autoJob.BlueprintID);
            // Debug.Log($"{Key}.{blueprint.Key}");
            PopulateDefaultPermissions(autoJob.BlueprintID, autoJob.ProhibitedResources, DefaultProhibitedResources, true);
            PopulateDefaultPermissions(autoJob.BlueprintID, autoJob.PermittedResources, DefaultProhibitedResources, false);
            PopulateDefaultPermissions(autoJob.BlueprintID, autoJob.ProhibitedLocations, DefaultProhibitedLocations, true);
            PopulateDefaultPermissions(autoJob.BlueprintID, autoJob.PermittedLocations, DefaultProhibitedLocations, false);

            if (!string.IsNullOrEmpty(blueprint.LocationID) &&
                    DataManager.TryGetTagObject(blueprint.LocationID, out var locationObj))
            {
                //Debug.Log("Location obj: " + locationObj.Key);
                bool addToCommonProps = false;

                if (locationObj is GDEBlocksData)
                {
                    addToCommonProps = true;
                }
                else if (locationObj is GDETagsData tag)
                {
                    List<ITagObject> tagObjects = DataManager.GetTagObjectsByTag(tag.Key);
                    bool locationTagHasBlock = false;

                    for (int j = 0; j < tagObjects.Count; j++)
                    {
                        if (tagObjects[j] is GDEBlocksData)
                        {
                            locationTagHasBlock = true;
                            break;
                        }
                    }

                    if (!locationTagHasBlock)
                    {
                        List<ITagObject> blocks = DataManager.GetTagObjects<GDEBlocksData>();

                        for (int j = 0; !locationTagHasBlock && j < blocks.Count; j++)
                        {
                            if (blocks[j] is GDEBlocksData block &&
                                block.Triggers != null &&
                                block.Triggers.Length > 0)
                            {
                                for (int k = 0; !locationTagHasBlock && k < block.Triggers.Length; k++)
                                {
                                    if (block.Triggers[k].ID == tag.Key)
                                    {
                                        locationTagHasBlock = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (locationTagHasBlock)
                    {
                        addToCommonProps = true;
                    }
                }

                if (addToCommonProps)
                {
                    if (CommonPropsSet.Add(blueprint.LocationID))
                    {
                        CommonProps.Add(blueprint.LocationID);
                    }
                }
            }
        }

        if (CommonProps.Count > 0)
        {
            CommonProps = CommonProps.OrderBy((string propID) =>
            {
                return DataManager.GetTagObject(propID).TooltipOrder;
            }).ToList();
        }

        base.OnLoaded();
    }

    private static void PopulateDefaultPermissions(string blueprintID, string[] tagObjIDs, Dictionary<string, Dictionary<string, bool>> defaultPermissions, bool prohibit)
    {
        if (tagObjIDs == null || tagObjIDs.Length == 0)
        {
            return;
        }

        for (int j = 0; j < tagObjIDs.Length; j++)
        {
            string tagObjID = tagObjIDs[j];

            if (string.IsNullOrEmpty(tagObjID))
            {
                continue;
            }

            if (!defaultPermissions.TryGetValue(blueprintID, out Dictionary<string, bool> permissions))
            {
                permissions = new Dictionary<string, bool>();
                defaultPermissions[blueprintID] = permissions;
            }

            if (DataManager.TryGetTagGroup(tagObjID, out var tagsByGroup))
            {
                for (int k = 0; k < tagsByGroup.Count; k++)
                {
                    List<ITagObject> tagObjects = DataManager.GetTagObjectsByTag(tagsByGroup[k].TagID);

                    for (int l = 0; l < tagObjects.Count; l++)
                    {
                        if (!permissions.ContainsKey(tagObjects[l].Key))
                        {
                            permissions.Add(tagObjects[l].Key, prohibit);
                        }
                        else
                        {
                            permissions[tagObjects[l].Key] = prohibit;
                        }

                        // Debug.Log($"{tagObjID}.{tagObjects[l].Key} = {prohibit}");
                    }
                }
            }
            else if (DataManager.TryGetTagData(tagObjID, out var tagData) && DataManager.TryGetTagObjectsByTag(tagData.TagID, out var tagObjectsByTag))
            {
                for (int l = 0; l < tagObjectsByTag.Count; l++)
                {
                    if (!permissions.ContainsKey(tagObjectsByTag[l].Key))
                    {
                        permissions.Add(tagObjectsByTag[l].Key, prohibit);
                    }
                    else
                    {
                        permissions[tagObjectsByTag[l].Key] = prohibit;
                    }

                    // Debug.Log($"{tagObjID}.{tagObjectsByTag[l].Key} = {prohibit}");
                }
            }
            else if (DataManager.TryGetTagObject(tagObjID, out var _))
            {
                if (!permissions.ContainsKey(tagObjID))
                {
                    permissions.Add(tagObjID, prohibit);
                }
                else
                {
                    permissions[tagObjID] = prohibit;
                }

                // Debug.Log($"{tagObjID} = {prohibit}");
            }
            else
            {
                Debug.LogError($"Invalid tag object ID '{tagObjID}' in prohibited/permitted list for auto job '{blueprintID}'");
            }
        }
    }
#endif
}
