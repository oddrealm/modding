
[System.Serializable]
public struct TagObjectSetting
{
    public string TagObjectKey;
    public int Min;
    public int Max;

    public TagObjectSetting(string tagObjectKey, int min, int max)
    {
        TagObjectKey = tagObjectKey;
        Min = min;
        Max = max;
    }

    public TagObjectSetting(string tagObjectKey)
    {
        TagObjectKey = tagObjectKey;

        // No restrictions.
        Min = -1;
        Max = -1;
    }
}
