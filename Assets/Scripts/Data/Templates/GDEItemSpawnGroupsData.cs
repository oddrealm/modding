using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemSpawnGroups")]
public class GDEItemSpawnGroupsData : ScriptableObject
{
	[System.Serializable]
	public class Spawn
    {
        public string ItemID = "item_wood_log";
        public string GenerateNameKey;
        public int Count = 1;
        public int SpawnRate = 1000;
        public bool RandomizeCount;
        public int MaxValue = int.MaxValue;
        public int RandomIDRarity = -1;

        [System.NonSerialized]
        public int TempMod;
    }

    public string Key;
    public string FriendlyName = "";

    public Spawn[] Spawns;

}
