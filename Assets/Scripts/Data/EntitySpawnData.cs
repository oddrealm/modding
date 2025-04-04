
using System.Collections.Generic;

[System.Serializable]
public struct EntitySpawnData
{
    public string EntityID;
    public InstanceUID NationUID;
    public string ProfessionID;
    public string FactionID;
    public string GenderID;
    public string TuningID;
    public string AppearanceID;
    public EntityAgeTypes AgeType;
    public int StartingAgeMins;
    public SimTime StartingSimTime;
    public string Tag;
    public int Count;
    public RandomChance Chance;
    public string SpawnScenario;
    public bool AutoGenEquipment;
    public bool AutoGenInventory;
    public bool DisableItemEquipFromTuning;
    public InstanceUID[] Parents;
    public List<EntitySpawnEquippedItem> EquippedItems;
    public List<EntitySpawnInventoryItem> InventoryItems;
    public List<string> AddedStatuses;

#if ODD_REALM_APP
    private static GameSession _session;

    public static void Init(GameSession session)
    {
        _session = session;
    }

    public static EntitySpawnData NewDefault(string entityID, string factionID = "", InstanceUID nationUID = default, string tuningID = "")
    {
        return new EntitySpawnData()
        {
            EntityID = entityID,
            NationUID = nationUID,
            FactionID = factionID,
            ProfessionID = "profession_random",
            GenderID = "priority",
            TuningID = tuningID,
            AppearanceID = "",
            AgeType = EntityAgeTypes.ADULT,
            Tag = "",
            Count = 1,
            Chance = new RandomChance(),
            SpawnScenario = "",
            AutoGenEquipment = true,
            AutoGenInventory = true,
            DisableItemEquipFromTuning = false,
            Parents = null,
            StartingAgeMins = -1,
            StartingSimTime = 0,
            EquippedItems = null,
            InventoryItems = null,
            AddedStatuses = null
        };
    }

    public static EntitySpawnData NewFromSpawnTagObj(GDEEntitiesData entityData, NewTagObjParams newTagObjData)
    {
        InstanceUID[] parents = null;
        string faction = "";

        if (!newTagObjData.sourceUID.IsNULL && _session.Entities.TryGetEntityByUID(newTagObjData.sourceUID, out var parent))
        {
            parents = new InstanceUID[2];

            parents[0] = parent.UID;
            parents[1] = parent.Family.CompanionUID;

            faction = parent.Faction.Data.Key;
        }

        return new EntitySpawnData()
        {
            EntityID = entityData.Key,
            NationUID = default,
            FactionID = faction,
            ProfessionID = "profession_random",
            GenderID = "priority",
            TuningID = "",
            AppearanceID = "",
            AgeType = EntityAgeTypes.NEWBORN,
            Tag = "",
            Count = 1,
            Chance = new RandomChance(),
            SpawnScenario = "",
            AutoGenEquipment = false,
            AutoGenInventory = false,
            DisableItemEquipFromTuning = false,
            Parents = parents,
            StartingAgeMins = newTagObjData.simState.SimAge,
            StartingSimTime = 0,
            EquippedItems = null,
            InventoryItems = null,
            AddedStatuses = null
        };
    }

    public static EntitySpawnData NewFromSpawn(GDEEntitySpawnGroupsData.Spawn spawn)
    {
        return new EntitySpawnData()
        {
            EntityID = spawn.EntityId,
            NationUID = default,
            FactionID = spawn.EntityFaction,
            ProfessionID = spawn.EntityProfession,
            GenderID = spawn.EntityGender,
            TuningID = spawn.EntityTuning,
            AppearanceID = spawn.EntityAppearance,
            AgeType = spawn.AgeType,
            Tag = spawn.EntityTag,
            Count = spawn.EntityCount,
            Chance = spawn.SpawnRate == 1000 ? new RandomChance() : new RandomChance((uint)spawn.SpawnRate, 1000),
            SpawnScenario = spawn.ScenarioID,
            AutoGenEquipment = spawn.AutoGenEquipment,
            AutoGenInventory = spawn.AutoGenInventory,
            DisableItemEquipFromTuning = spawn.DisableItemEquipFromTuning,
            Parents = null,
            StartingAgeMins = -1,
            StartingSimTime = 0,
            EquippedItems = spawn.EquippedItems == null ? null : new List<EntitySpawnEquippedItem>(spawn.EquippedItems),
            InventoryItems = spawn.InventoryItems == null ? null : new List<EntitySpawnInventoryItem>(spawn.InventoryItems),
            AddedStatuses = spawn.AddedStatuses == null ? null : new List<string>(spawn.AddedStatuses)
        };
    }
#endif
}
