using Assets.GameData;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Entities")]
public class GDEEntitiesData : Scriptable, ISimulationData
{

    [System.Serializable]
    public struct AttributeTuning
    {
        public string AttributeID;
        public int StartingBase;
    }

    [System.Serializable]
    public struct SkillPermission
    {
        public string SkillID;
        public bool PlayerCanEdit;
        public bool EnabledByDefault;
    }

    [System.Serializable]
    public struct ProfessionPermission
    {
        public string ProfessionID;
        public bool CanAssign;
        public bool CanSpawnWith;
    }

    [System.Serializable]
    public struct EntityAutoJob
    {
        public string BlueprintID;
        public AutoJobSettings Settings;
    }

    [System.Serializable]
    public struct DietSettings
    {
        public string TagID;
        public string TagObjectID;
        public int Priority;
        public BuffData[] Buffs;
        public string[] Statuses;

        public bool HasAffects
        {
            get
            {
                return HasBuffs || HasStatuses;
            }
        }

        public bool HasBuffs
        {
            get
            {
                return Buffs != null && Buffs.Length > 0;
            }
        }

        public bool HasStatuses
        {
            get
            {
                return Statuses != null && Statuses.Length > 0;
            }
        }
    }

    public string Race = "";
    [Header("Lifetime Minutes (-1 = disabled)")]
    public int MaxLifeTime = -1;
    [Header("Max amount to simulate for world gen")]
    public int MaxDefaultSimTime = 24 * 60 * 10;
    [Header("Simulation ID")]
    public string EntitySimulationID = "";
    public string SizeTagID = "tag_entity_size_medium";
    public string[] LanguagesSpoken = new[] { "language_engel" };
    public string[] DefaultLeaderRoles = new[] { "" };
    public EntityCompanionTypes CompanionType = 0;
    public EntityCompanionPermanenceTypes CompanionPermanenceType = 0;
    public EntityReproductionTypes ReproductionType = 0;
    public int MaxChildren = 3;
    public string OnMateStatus = "";
    public string OnMateFX = "fx_entity_union";
    public string OnMateSFX = "sfx_mate";
    public bool CanBreathUnderWater = false;
    public bool TakeLastNameOfFamilyMembers = false;
    public int ReproductionIntervalMinutes = 0;
    public string RequiredReproductionRoom = "";
    public string GenerateNameKey = "";
    public string[] Attacks = System.Array.Empty<string>();
    public List<SkillPermission> SkillPermissions = new();
    public List<ProfessionPermission> ProfessionPermissions = new();
    public string[] PermittedItemSlots;
    public HashSet<string> PermittedItemSlotsHash = new();
    public PathingTypes PathingType = 0;
    public string DefaultTuning = "";
    public string DeathItemSpawnGroup = "";
    public List<string> PreyRaces = new();
    public List<string> ProhibitedStatuses = new();
    public List<string> Statuses = new();
    public List<string> Ages = new();
    public List<DietSettings> Diets = new();
    public List<EntityAutoJob> AutoJobs = new();
    public List<AttributeTuning> Attributes = new();
    public List<string> Biomes = new();
    public HashSet<string> BiomesHash = new();
    public SimOptions[] StateOptions;

    private GDESimulationData _simData;
    private readonly string[] _simStates = new string[]
    {
    };

    public Dictionary<string, List<DietSettings>> DietAffectsByTagObjectID { get; private set; } = new Dictionary<string, List<DietSettings>>();
    public Dictionary<string, ProfessionPermission> ProfessionPermissionsByID { get; private set; } = new Dictionary<string, ProfessionPermission>();
    public Dictionary<string, SkillPermission> SkillPermissionsByID { get; private set; } = new Dictionary<string, SkillPermission>();
    public GDETagsData SizeTag { get; private set; }
    public string SimulationID { get { return EntitySimulationID; } }
    public GDESimulationData Simulation { get { return _simData; } }
    public SimTime MaxSimTime { get { return MaxDefaultSimTime; } }
    public HashSet<EntityAgeTypes> PermittedAgeTypes { get; private set; } = new HashSet<EntityAgeTypes>();

#if ODD_REALM_APP
    public override void Init()
    {
        base.Init();

        BiomesHash.Clear();

        for (int i = 0; i < Biomes.Count; i++)
        {
            BiomesHash.Add(Biomes[i]);
        }
    }

    public override void OnLoaded()
    {
#if DEV_TESTING
        for (int i = 0; i < AutoJobs.Count; i++)
        {
            GDEBlueprintsData blueprint = DataManager.GetTagObject<GDEBlueprintsData>(AutoJobs[i].BlueprintID);
            if (!blueprint.DisposeIfNoSourceEntity)
            {
                Debug.LogError(blueprint.Key + " needs to dispose if no source entity!");
            }
        }
#endif
        SkillPermissionsByID.Clear();

        for (int i = 0; i < SkillPermissions.Count; i++)
        {
            if (!SkillPermissionsByID.ContainsKey(SkillPermissions[i].SkillID))
            {
                SkillPermissionsByID.Add(SkillPermissions[i].SkillID, SkillPermissions[i]);
            }
            else
            {
                Debug.LogError($"Duplicate skill permission: {Key}.{SkillPermissions[i].SkillID}");
            }
        }

        PermittedAgeTypes.Clear();

        for (int i = 0; i < Ages.Count; i++)
        {
            GDEEntityAgeData ageData = DataManager.GetTagObject<GDEEntityAgeData>(Ages[i]);
            PermittedAgeTypes.Add(ageData.AgeType);
        }

        ProfessionPermissionsByID.Clear();

#if DEV_TESTING
        List<ITagObject> professions = DataManager.GetTagObjects<GDEProfessionData>();

        for (int i = 0; i < professions.Count; i++)
        {
            bool hasProfession = false;

            for (int j = 0; j < ProfessionPermissions.Count; j++)
            {
                if (ProfessionPermissions[j].ProfessionID == professions[i].Key)
                {
                    hasProfession = true;
                    break;
                }
            }

            if (!hasProfession)
            {
                Debug.LogError($"Adding profession permission: {Key}.{professions[i].Key}");
                ProfessionPermissions.Add(new ProfessionPermission()
                {
                    ProfessionID = professions[i].Key,
                    CanAssign = true,
                    CanSpawnWith = true
                });
            }
        }

        //GDERacesData race = DataManager.GetTagObject<GDERacesData>(Race);

        //if (race.Intelligence == "intelligence_non_sapient")
        //{
        //    // Disable all professions but animal
        //    for (int i = 0; i < professions.Count; i++)
        //    {
        //        if (professions[i].Key != "profession_animal")
        //        {
        //            ProfessionPermission permission = new ProfessionPermission()
        //            {
        //                ProfessionID = professions[i].Key,
        //                CanAssign = false,
        //                CanSpawnWith = false
        //            };

        //            SetProfessionPermission(permission);
        //            Debug.LogError($"Disabling profession permission: {Key}.{professions[i].Key}");
        //        }
        //    }
        //}
#endif

        for (int i = 0; i < ProfessionPermissions.Count; i++)
        {
            if (!ProfessionPermissionsByID.ContainsKey(ProfessionPermissions[i].ProfessionID))
            {
                ProfessionPermissionsByID.Add(ProfessionPermissions[i].ProfessionID, ProfessionPermissions[i]);
            }
            else
            {
                Debug.LogError($"Duplicate Profession permission: {Key}.{ProfessionPermissions[i].ProfessionID}");
            }
        }

        DietAffectsByTagObjectID.Clear();

        for (int i = 0; i < Diets.Count; i++)
        {
            if (!Diets[i].HasAffects) { continue; }

            if (!string.IsNullOrEmpty(Diets[i].TagObjectID))
            {
                if (!DietAffectsByTagObjectID.ContainsKey(Diets[i].TagObjectID))
                {
                    DietAffectsByTagObjectID.Add(Diets[i].TagObjectID, new List<DietSettings>() { Diets[i] });
                }
                else
                {
                    DietAffectsByTagObjectID[Diets[i].TagObjectID].Add(Diets[i]);
                }
            }
            else if (DataManager.TryGetTagObjectsByTag(Diets[i].TagID, out var tagObjs))
            {
                for (int j = 0; j < tagObjs.Count; j++)
                {
                    if (!DietAffectsByTagObjectID.ContainsKey(tagObjs[j].Key))
                    {
                        DietAffectsByTagObjectID.Add(tagObjs[j].Key, new List<DietSettings>() { Diets[i] });
                    }
                    else
                    {
                        DietAffectsByTagObjectID[tagObjs[j].Key].Add(Diets[i]);
                    }
                }
            }
            else
            {
                Debug.LogError($"Diet tag ID has no tag objects: {Key}.{Diets[i].TagID}");
            }
        }

        List<AttributeTuning> passed = new();

        for (int i = 0; i < Attributes.Count; i++)
        {
            if (!DataManager.GetTagObject(Attributes[i].AttributeID).IsNULL)
            {
                passed.Add(Attributes[i]);
            }
        }

        Attributes.Clear();

        for (int i = 0; i < passed.Count; i++)
        {
            Attributes.Add(passed[i]);
        }

        PermittedItemSlotsHash.Clear();

        for (int i = 0; i < PermittedItemSlots.Length; i++)
        {
            PermittedItemSlotsHash.Add(PermittedItemSlots[i]);
        }

        if (!string.IsNullOrEmpty(EntitySimulationID))
        {
            _simData = DataManager.GetTagObject<GDESimulationData>(EntitySimulationID);
        }

        SizeTag = DataManager.GetTagObject<GDETagsData>(SizeTagID);

        base.OnLoaded();
    }
#endif

    public bool TryGetSkillPermission(string skillID, out SkillPermission permission)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            for (int i = 0; i < SkillPermissions.Count; i++)
            {
                if (SkillPermissions[i].SkillID == skillID)
                {
                    permission = SkillPermissions[i];
                    return true;
                }
            }
        }
#endif
        return SkillPermissionsByID.TryGetValue(skillID, out permission);
    }

    public void SetSkillPermission(SkillPermission permission)
    {
        for (int i = 0; i < SkillPermissions.Count; i++)
        {
            if (SkillPermissions[i].SkillID != permission.SkillID)
            {
                continue;
            }

            SkillPermissions[i] = permission;

            return;
        }

        SkillPermissions.Add(permission);

        if (!SkillPermissionsByID.ContainsKey(permission.SkillID))
        {
            SkillPermissionsByID.Add(permission.SkillID, permission);
        }
        else
        {
            SkillPermissionsByID[permission.SkillID] = permission;
        }
    }

    public SkillPermission GetSkillPermissionByID(string skillID)
    {
        if (TryGetSkillPermission(skillID, out var permission))
        {
            return permission;
        }

        SkillPermission defaultPermission = new()
        {
            SkillID = skillID,
            PlayerCanEdit = true,
            EnabledByDefault = false
        };

        return defaultPermission;
    }

    public bool TryGetProfessionPermission(string ProfessionID, out ProfessionPermission permission)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            for (int i = 0; i < ProfessionPermissions.Count; i++)
            {
                if (ProfessionPermissions[i].ProfessionID == ProfessionID)
                {
                    permission = ProfessionPermissions[i];
                    return true;
                }
            }
        }
#endif
        return ProfessionPermissionsByID.TryGetValue(ProfessionID, out permission);
    }

    public void SetProfessionPermission(ProfessionPermission permission)
    {
        for (int i = 0; i < ProfessionPermissions.Count; i++)
        {
            if (ProfessionPermissions[i].ProfessionID != permission.ProfessionID)
            {
                continue;
            }

            ProfessionPermissions[i] = permission;

            return;
        }

        ProfessionPermissions.Add(permission);

        if (!ProfessionPermissionsByID.ContainsKey(permission.ProfessionID))
        {
            ProfessionPermissionsByID.Add(permission.ProfessionID, permission);
        }
        else
        {
            ProfessionPermissionsByID[permission.ProfessionID] = permission;
        }
    }

    public ProfessionPermission GetProfessionPermissionByID(string ProfessionID)
    {
        if (TryGetProfessionPermission(ProfessionID, out var permission))
        {
            return permission;
        }

        ProfessionPermission defaultPermission = new()
        {
            ProfessionID = ProfessionID,
            CanAssign = true,
            CanSpawnWith = true
        };

        return defaultPermission;
    }

    public bool IsPermittedProfession(string professionID)
    {
        if (ProfessionPermissionsByID.TryGetValue(professionID, out var permission))
        {
            return permission.CanSpawnWith;
        }

        return false;
    }

    public bool TryGetDietAffectsByTagObjID(string tagObjectID, out List<DietSettings> dietAffects)
    {
        return DietAffectsByTagObjectID.TryGetValue(tagObjectID, out dietAffects);
    }

    public bool CanShowInProgressUI
    {
        get
        {
            return true;
        }
    }

    public void SetSimulationID(string simID)
    {
        EntitySimulationID = simID;
    }

    public string[] GetSimStates()
    {
        return _simStates;
    }

    public SimOptions[] GetOptions()
    {
        return StateOptions;
    }
}
