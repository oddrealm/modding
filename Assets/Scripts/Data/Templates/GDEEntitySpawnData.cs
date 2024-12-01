using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntitySpawn")]
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
