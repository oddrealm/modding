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
        public TagObjectSetting[] ResourcePermissions;
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

    [Header("Room Management")]
    public bool ShowProduction = true;
    public bool ShowStockpile = true;
    public bool ShowOccupants = true;
    public bool ShowMarket = true;

    public override string ObjectTypeDisplay { get { return "Rooms"; } }
    public List<string> CommonProps { get; private set; } = new List<string>();
    public HashSet<string> CommonPropsSet { get; private set; } = new HashSet<string>();

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

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        CommonProps.Clear();
        CommonPropsSet.Clear();

        for (int i = 0; DefaultAutoJobs != null && i < DefaultAutoJobs.Count; i++)
        {
            GDEBlueprintsData blueprint = DataManager.GetTagObject<GDEBlueprintsData>(DefaultAutoJobs[i].BlueprintID);

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

#if UNITY_EDITOR
            if (DefaultAutoJobs[i].ResourcePermissions == null ||
                DefaultAutoJobs[i].ResourcePermissions.Length == 0)
            {
                continue;
            }

            for (int j = 0; j < DefaultAutoJobs[i].ResourcePermissions.Length; j++)
            {
                if (!string.IsNullOrEmpty(DefaultAutoJobs[i].ResourcePermissions[j].TagObjectKey) &&
                    !DataManager.TagObjectExists(DefaultAutoJobs[i].ResourcePermissions[j].TagObjectKey))
                {
                    Debug.LogError(Key + " Auto Job " + DefaultAutoJobs[i].BlueprintID + " could find tag object for: " + DefaultAutoJobs[i].ResourcePermissions[j].TagObjectKey);
                }
            }
#endif
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
#endif
}
