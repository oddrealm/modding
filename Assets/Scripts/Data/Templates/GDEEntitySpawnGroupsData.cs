using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntitySpawnGroups")]
public class GDEEntitySpawnGroupsData : ScriptableObject
{
    public string Key;

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
        public FactionTypes EntityFaction = FactionTypes.NEUTRAL;
        public string EntityTuning = "";
        public int SpawnRate = 1000;
        public EntityAgeTypes AgeType = EntityAgeTypes.ADULT;
        public string ScenarioID = "";
        public string SchemaID = "";
    }
}
