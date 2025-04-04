using UnityEngine;

[System.Serializable]
public struct GlobalBuff
{
    public string ID;
    public string RequiredTargetTagObjectID;
    [System.NonSerialized]
    public InstanceUID UID;
    public BuffData Buff;
    [System.NonSerialized]
    public string SourceTagObjectID;
}