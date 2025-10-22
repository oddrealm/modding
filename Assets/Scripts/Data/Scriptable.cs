using Assets.GameData;
using System.Collections.Generic;
using UnityEngine;
#if DEV_TESTING
using System.Linq;
#endif

public class Scriptable : ScriptableObject, ITagObject
{
    public bool DEBUG;
    public string TooltipID = "";
    public List<string> TagIDs = new();
    public List<string> DiscoveryDependencies = new();
    public bool VisibleToPlayer = true;

    public HashSet<string> TagIDsHash { get; private set; } = new HashSet<string>();
    public HashSet<string> DiscoveryDependenciesHash { get; private set; } = new HashSet<string>();
    public int TagCount { get { return TagIDs.Count; } }
    public bool ShowToPlayer => VisibleToPlayer;
    public string Key
    {
        get
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(_key))
            {
                _key = name;
            }
#endif
            return _key;
        }
    }

    private string _key;

    public virtual bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking();
        return false;
    }

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
        if (string.IsNullOrEmpty(tag)) { return; }
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

    // Used to determine ObjectIndex when loading game data
    public virtual string OrderKey { get; private set; }
    // Set on game data load based on tooltip name and groups
    public int OrderIndex { get; set; }
    public int DataIndex { get; set; }
    public Color MapColor = Color.white;
    public virtual Color MinimapColor { get { return MapColor; } }
    public virtual bool ShowOnMinimap { get { return true; } }
    public virtual bool ShowMinimapCutoutColor { get { return false; } }

#if ODD_REALM_APP
    public static readonly List<ITag> NULL_TAG_LIST = new();
    public static readonly List<ITagObject> NULL_TAG_OBJECT_LIST = new();
    public static readonly UintArray NULL_UID_ARRAY = new(true);
    public static readonly NullTag NULL_TAG = new();
    public static readonly NullTagObject NULL_TAG_OBJECT = new();
    public static readonly NullTagObjectInstance NULL_TAG_OBJECT_INSTANCE = new();
    public static readonly List<string> NULL_ID_LIST = new();
    public static readonly HashSet<string> NULL_ID_SET = new();
    public static readonly List<InstanceUID> NULL_UID_LIST = new();
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
    public string TooltipDiscoveryHint { get { return TooltipData.DiscoveryHint; } }
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

    public void UpdateKey()
    {
        _key = name;
    }

#if ODD_REALM_APP
    public virtual void Init()
    {
        UpdateKey();
    }

    public virtual void SetOrderKey(string orderKey)
    {
        OrderKey = orderKey;
    }

    public virtual void OnReordered(int dataIndex)
    {
        DataIndex = dataIndex;
    }

    public void EnsureTag(string tagID)
    {
#if DEV_TESTING
        if (string.IsNullOrEmpty(tagID)) { return; }
        if (TagIDs.Contains(tagID)) { return; }

        TagIDs.Add(tagID);
        TagIDsHash.Add(tagID);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
#endif
    }

    public virtual void OnLoaded()
    {
        SetOrderKey(TooltipName);

#if DEV_TESTING
        // if (DiscoveryDependencies.Count > 0)
        // {
        //     Debug.LogError(_key + " has discovery dependencies!");
        // }
        if (TagIDs.Count > 50)
        {
            Debug.LogError(_key + " has a high tag count!");
        }

        // Remove duplicates from TagIDs list.
        // var tagIDs = TagIDs.Distinct().ToList();
        //
        // if (tagIDs.Count != TagIDs.Count)
        // {
        //     UnityEditor.EditorUtility.SetDirty(this);
        //     TagIDs = tagIDs;
        // }

#endif

        TagIDsHash.Clear();

        if (TagIDs.Count > 0)
        {
            TagIDsHash.UnionWith(TagIDs);
        }
    }
#endif
}
