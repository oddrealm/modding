using Assets.GameData;
using System.Collections.Generic;
using UnityEngine;

public class Scriptable : ScriptableObject, ITagObject
{
    public string Key
    {
        get
        {
#if UNITY_EDITOR
            if (_key != name)
            {
                _key = name;
            }
#endif
            return _key;
        }
    }

    private string _key;

    public string TooltipID = "";
    public string GroupID = "";
    public bool DEBUG;

    public List<string> TagIDs = new List<string>();
    public List<string> DiscoveryDependencies = new List<string>();

    [System.NonSerialized]
    public HashSet<string> TagIDsHash = new HashSet<string>();

    [System.NonSerialized]
    public HashSet<string> DiscoveryDependenciesHash = new HashSet<string>();

    public virtual bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking();
        return false;
    }

    public int TagCount { get { return TagIDs.Count; } }

    public bool HasTag(string tagID)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return TagIDs.Contains(tagID);
        }
#endif
        return TagIDsHash.Contains(tagID);
    }

    public void AddTag(string tag)
    {
        if (TagIDs.Contains(tag)) { return; }
        TagIDs.Add(tag);
    }

    public void RemoveTag(string tag)
    {
        TagIDs.Remove(tag);
    }

    public ITag GetTag(int index)
    {
        return GetTag(TagIDs[index]);
    }

    public string SetTagID(int index, string tagID)
    {
        if (index >= TagIDs.Count)
        {
            return tagID;
        }

        return TagIDs[index] = tagID;
    }

    public string GetTagID(int index)
    {
        return TagIDs[index];
    }

    public ITag GetTag(string tagID)
    {
#if ODD_REALM_APP
        return DataManager.GetTagData(tagID);
#else
        return null;
#endif
    }

    public ITagObject GetTagTagObject(int index)
    {
#if ODD_REALM_APP
        return DataManager.GetTagObject(TagIDs[index]);
#else 
        return null;
#endif
    }

    public List<string> GetTagIDs()
    {
        return TagIDs;
    }

    public List<string> GetDiscoveryDependencies()
    {
        return DiscoveryDependencies;
    }

    public virtual bool IsNULL { get { return false; } }

    public string ObjectType
    {
        get { return GetType().Name; }
    }

    public virtual string ObjectTypeDisplay
    {
        get { return ObjectType; }
    }

    public string ObjectGroup
    {
        get { return GroupID; }
    }

    // Used to determine ObjectIndex when loading game data
    public virtual string OrderKey { get; private set; }
    // Set on game data load based on tooltip name and groups
    public int ObjectIndex { get; set; }
    public int DataIndex { get; set; }

    public Color MapColor = Color.white;

    public virtual Color MinimapColor { get { return MapColor; } }
    public virtual bool ShowOnMinimap { get { return true; } }
    public virtual bool ShowMinimapCutoutColor { get { return false; } }

#if ODD_REALM_APP
    public static readonly List<ITag> NULL_TAG_LIST = new List<ITag>();
    public static readonly List<ITagObject> NULL_TAG_OBJECT_LIST = new List<ITagObject>();
    public static readonly UintArray NULL_UID_ARRAY = new UintArray(true);
    public static readonly NullTag NULL_TAG = new NullTag();
    public static readonly NullTagObject NULL_TAG_OBJECT = new NullTagObject();
    public static readonly NullTagObjectInstance NULL_TAG_OBJECT_INSTANCE = new NullTagObjectInstance();
    public static readonly List<string> NULL_ID_LIST = new List<string>();
    public static readonly List<InstanceUID> NULL_UID_LIST = new List<InstanceUID>();
#endif

    #region ITooltipFormat
    public virtual GDETooltipsData TooltipData
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (string.IsNullOrEmpty(TooltipID))
                {
                    return DataUtility.ImportDataSingle<GDETooltipsData>("tooltip_missing");
                }

                GDETooltipsData offlineTooltipData = DataUtility.ImportDataSingle<GDETooltipsData>(TooltipID);

                if (offlineTooltipData == null)
                {
                    return DataUtility.ImportDataSingle<GDETooltipsData>("tooltip_missing");
                }

                return offlineTooltipData;
            }
#endif

#if ODD_REALM_APP
            if (string.IsNullOrEmpty(TooltipID))
            {
                return DataManager.GetTagObject<GDETooltipsData>("tooltip_missing");
            }

            GDETooltipsData tooltipData = DataManager.GetTagObject<GDETooltipsData>(TooltipID);

            if (tooltipData == null)
            {
                return DataManager.GetTagObject<GDETooltipsData>("tooltip_missing");
            }

            return tooltipData;
#else
            return null;
#endif
        }
    }


    public string TooltipInlineAndName { get { return TooltipData.InlineAndName; } }
    public string TooltipInlineIcon { get { return TooltipData.InlineIcon; } }
    public string TooltipName { get { return TooltipData.Name; } }
    public string TooltipAction { get { return TooltipData.Action; } }
    public string TooltipNamePlural { get { return TooltipData.NamePlural; } }
    public string TooltipIcon { get { return TooltipData.Icon; } }
    public string TooltipType { get { return TooltipData.Type; } }
    public string TooltipDescription { get { return TooltipData.Description; } }
#if ODD_REALM_APP
    public Sprite TooltipIconSprite { get { return GlobalSettingsManager.GetIcon(TooltipData.Icon); } }
#else
    public Sprite TooltipIconSprite { get { return null; } }
#endif
    public Color TooltipTextColor { get { return TooltipData.TextColor; } }
    public Color TooltipTypeColor { get { return TooltipData.TypeColor; } }
    public int TooltipOrder { get { return TooltipData.Order; } }


    public TooltipUID TooltipUID { get { return TooltipData.TooltipUID; } }

    #endregion

#if ODD_REALM_APP
    public virtual void Init()
    {
        _key = name;
    }

    public virtual void SetOrderKey(string orderKey)
    {
        OrderKey = orderKey;
    }

    public virtual void OnReordered(int dataIndex)
    {
        DataIndex = dataIndex;
    }

    public virtual void OnLoaded()
    {
        SetOrderKey(TooltipName);

#if DEV_TESTING
        if (TagIDs.Count > 50)
        {
            Debug.LogError(_key + " has a high tag count!");
        }
#endif

        TagIDsHash.Clear();

        if (TagIDs.Count > 0)
        {
            TagIDsHash.UnionWith(TagIDs);
        }
    }
#endif
}
