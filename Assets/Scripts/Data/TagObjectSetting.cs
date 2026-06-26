
[System.Serializable]
public struct TagObjectSetting
{
    public string TagID;
    public string TagObjectKey;
    public bool Prohibit;

    public readonly bool IsPermitted { get { return !Prohibit; } }
}
