

[System.Serializable]
public struct BuffData
{
    public BuffData(string targetID, int amount, int max, bool permanent)
    {
        TargetID = targetID;
        Amount = amount;
        Max = max;
        Permanent = permanent;
    }

    public static readonly BuffData NULL = new BuffData("", 0, 0, false);

    public string TargetID;
    public int Amount;
    public int Max;
    public bool Permanent; // If false, this will be tracked in Entity Buffs.

#if ODD_REALM_APP
    public bool IsNULL { get { return string.IsNullOrEmpty(TargetID); } }

    public ITagObject TargetObj { get { return string.IsNullOrEmpty(TargetID) ? Scriptable.NULL_TAG_OBJECT : DataManager.GetTagObject(TargetID); } }

    public override string ToString()
    {
        return $"BuffData({TargetID} - (Amount:{Amount} Max:{Max}) - Permanent:{Permanent})";
    }
#endif
}