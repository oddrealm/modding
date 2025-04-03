using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Landmark", order = 0)]
public class GDELandmarkData : Scriptable
{
    [System.Serializable]
    public struct PrefabSettings
    {
        public string PrefabID;
        public int MaxCount;
        public int MinCount;
    }

    public PrefabSettings[] Prefabs = new PrefabSettings[] { };
}
