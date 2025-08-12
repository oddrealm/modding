using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DefaultTracking
{
    public string TagID;
    public string TagObjectID;
    public bool HideIfZero;
    public bool StartDisabled;
    public bool PlayerEditDisabled;
    public string OnZeroNotif;
    public TrackingTypes TrackingType;

    public bool IsNULL
    {
        get
        {
            return string.IsNullOrEmpty(TagID) || string.IsNullOrEmpty(TagObjectID);
        }
    }
}

public interface ITagObject : ITooltipContent
{
    bool IsNULL { get; }
    string Key { get; }
    string ObjectType { get; }
    string ObjectTypeDisplay { get; }
    int OrderIndex { get; set; }
    string OrderKey { get; }
    int TagCount { get; }
    bool HasTag(string tagID);
    ITag GetTag(string tagID);
    ITag GetTag(int index);
    List<string> GetTagIDs();
    List<string> GetDiscoveryDependencies();
    Color MinimapColor { get; }
    bool ShowMinimapCutoutColor { get; }
    bool ShowOnMinimap { get; }

    bool TryGetDefaultTracking(out DefaultTracking tracking);
}