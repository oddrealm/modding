using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Entity/EntitySpawn", order = 23)]
public class GDEEntitySpawnData : Scriptable
{
    [System.Serializable]
    public struct Spawn
    {
        public string EntityID;
        public string FactionID;
        public string TuningID;
    }

    public Spawn[] Spawns;
}
