using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Entity/EntitySpawnGroups", order = 24)]
public class GDEEntitySpawnGroupsData : Scriptable
{
    public string FriendlyName = "Spawns:";
    public Spawn[] Spawns;

    [System.Serializable]
    public class Spawn
    {
        public string EntityId = "";
        public string EntityTag = "";
        public string EntityProfession = "profession_random";
        public string EntityGender = "priority";
        public int EntityCount = 1;
        public bool SameRoomAsPlayer;
        public string EntityFaction = "faction_player";
        public string EntityTuning = "";
        public string EntityAppearance = "";
        [Header("A random roll 1/1000")]
        [Range(1, 1000)]
        public int SpawnRate = 1000;
        public EntityAgeTypes AgeType = EntityAgeTypes.ADULT;
        public string ScenarioID = "";
        public string SchemaID = "";
        public bool SpawnEntityFromItem = false;
        public bool AutoGenEquipment = true;
        public bool AutoGenInventory = true;
        public bool DisableItemEquipFromTuning = false;
        public List<string> EquippedItems = new List<string>();
        public List<string> AddedStatuses = new List<string>();
    }
}
