

[System.Serializable]
public struct BuffData
{
    public BuffData(string targetID, string sourceID, int amount, int max, bool permanent)
    {
        TargetID = targetID;
        SourceID = sourceID;
        Amount = amount;
        Max = max;
        Permanent = permanent;
    }

    public static readonly BuffData NULL = new(string.Empty, string.Empty, 0, 0, false);

    public string TargetID;
    public string SourceID;
    public int Amount;
    public int Max;
    public bool Permanent; // If false, this will be tracked in Entity Buffs.

#if ODD_REALM_APP
    public readonly bool IsNULL { get { return string.IsNullOrEmpty(TargetID); } }

    public readonly ITagObject TargetObj { get { return string.IsNullOrEmpty(TargetID) ? Scriptable.NULL_TAG_OBJECT : DataManager.GetTagObject(TargetID); } }
    public readonly ITagObject SourceObj { get { return string.IsNullOrEmpty(SourceID) ? Scriptable.NULL_TAG_OBJECT : DataManager.GetTagObject(SourceID); } }
    public readonly bool HasSource { get { return !string.IsNullOrEmpty(SourceID); } }

    public override readonly string ToString()
    {
        return $"BuffData({TargetID} - (Amount:{Amount} Max:{Max}) - Permanent:{Permanent})";
    }
#endif
}
