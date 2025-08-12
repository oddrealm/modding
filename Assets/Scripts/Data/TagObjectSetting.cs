
[System.Serializable]
public struct TagObjectSetting
{
    public string TagID;
    public string TagObjectKey;
    public bool Prohibit;

    public bool IsPermitted { get { return !Prohibit; } }
}
