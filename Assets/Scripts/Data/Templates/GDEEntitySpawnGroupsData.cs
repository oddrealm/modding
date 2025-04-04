using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntitySpawnGroups")]
public class GDEEntitySpawnGroupsData : Scriptable
{
    public Spawn[] Spawns;

    [System.Serializable]
    public class Spawn
    {
        public string EntityId = "";
        public string EntityTag = "";
        public string EntityProfession = "profession_random";
        public string EntityGender = "priority";
        public int EntityCount = 1;
        public string EntityFaction = "faction_player";
        public string EntityTuning = "";
        public string EntityAppearance = "";
        [Header("A random roll 1/1000")]
        [Range(1, 1000)]
        public int SpawnRate = 1000;
        public EntityAgeTypes AgeType = EntityAgeTypes.ADULT;
        public string ScenarioID = "";
        public bool SpawnEntityFromItem = false;
        public bool AutoGenEquipment = true;
        public bool AutoGenInventory = true;
        public bool DisableItemEquipFromTuning = false;
        public List<EntitySpawnEquippedItem> EquippedItems = new List<EntitySpawnEquippedItem>();
        public List<EntitySpawnInventoryItem> InventoryItems = new List<EntitySpawnInventoryItem>();
        public List<string> AddedStatuses = new List<string>();
    }
}
