using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Biomes")]
public class GDEBiomesData : Scriptable
{
    public enum PopulationRatings
    {
        VERY_RARE = 0,
        RARE = 1,
        UNCOMMON = 2,
        COMMON = 3,
        VERY_COMMON = 4,
        COUNT = 5
    }

    [System.Serializable]
    public struct PopulationModifier
    {
        public float Modifier;
        public string TagObjID;

        public PopulationModifier(float mod, string tagObject)
        {
            Modifier = mod;
            TagObjID = tagObject;
        }
    }

    [System.Serializable]
    public struct DefaultPopulationRating
    {
        public PopulationRatings Rating;
        public string TagObjID;

        public DefaultPopulationRating(PopulationRatings rating, string tagObject)
        {
            Rating = rating;
            TagObjID = tagObject;
        }
    }

    [System.Serializable]
    public class TerrainLayer
    {
        [Header("Tag")]
        public string TagID = "tag_terrain_gen_dirt";

        [HideInInspector]
        public TagUID TagUID;

        public string Comment = "";

        [Header("Max Z")]
        [Range(0f, 1f)]
        public float Max;
    }

    [System.Serializable]
    public class OreLayer
    {
        public string Comment = "";

        [Header("Tags")]
        public string[] TagIDs = System.Array.Empty<string>();

        [HideInInspector]
        public TagUID[] TagUIDs = System.Array.Empty<TagUID>();

        [Header("Max Z")]
        [Range(0f, 1f)]
        public float Max;

        [Header("Min Z")]
        [Range(0f, 1f)]
        public float Min;
    }

    [System.Serializable]
    public class DungeonLayer
    {
        public static DungeonLayer NULL = new();

        public string Comment = "";

        [Header("Dungeon IDs (i.e., \"dungeon_default\")")]
        public string[] DungeonIDs = new string[] { "dungeon_default" };

        [HideInInspector]
        public GDEDungeonData[] DungeonsData;

        [Header("Max Z")]
        [Range(0f, 1f)]
        public float Max;

        [Header("Min Z")]
        [Range(0f, 1f)]
        public float Min;

        [HideInInspector]
        public int MaxZ { get; private set; }
        [HideInInspector]
        public int MinZ { get; private set; }
    }

    [System.Serializable]
    public class CaveLayer
    {
        public static CaveLayer NULL = new();

        public string Comment = "";

        [Header("Interior Spawn IDs (i.e., \"cave_default\")")]
        public string[] InteriorSpawnTags = new string[] { "cave_default" };

        [HideInInspector]
        public GDECaveData[] InteriorSpawns;

        [Header("Max Z")]
        [Range(0f, 1f)]
        public float Max;

        [Header("Min Z")]
        [Range(0f, 1f)]
        public float Min;

        [HideInInspector]
        public int MaxZ { get; private set; }
        [HideInInspector]
        public int MinZ { get; private set; }
    }

    [System.Serializable]
    public class TerrainGen
    {
        [HideInInspector]
        public TerrainLayer[] SpreadTerrainLayers;
        [HideInInspector]
        public OreLayer[] SpreadOreLayers;
        [HideInInspector]
        public CaveLayer[] SpreadCaveLayers;
        [HideInInspector]
        public DungeonLayer[] SpreadDungeonLayers;
        [HideInInspector]
        public GDEBiomeNoiseData TerrainNoise;
        [HideInInspector]
        public GDEBiomeNoiseData OreNoise;
        [HideInInspector]
        public GDEBiomeNoiseData PlantNoise;
        [HideInInspector]
        public GDEBiomeNoiseData CaveLowFreqNoise;
        [HideInInspector]
        public GDEBiomeNoiseData CaveHighFreqNoise;
        [HideInInspector]
        public GDEBiomeNoiseData StitchNoise;
        [HideInInspector]
        public TagUID SnowTagUID;

        [Header("Terrain Layers")]
        public TerrainLayer[] Layers;

        [Header("Ore Layers")]
        public OreLayer[] Ores;

        [Header("Cave Layers")]
        public CaveLayer[] Caves;

        [Header("Dungeon Layers")]
        public DungeonLayer[] Dungeons;

        [Header("Terrain Height Distribution")]
        public AnimationCurve TerrainDistributionCurve = new();

        [Header("Sim Obj Starting Age Dstribution")]
        public AnimationCurve SimStartAgeDistributionCurve = new();

        [Header("Noise")]
        public string NoiseID;

        [Header("River Bottom")]
        [Range(0f, 1f)]
        public float RiverBottom;

        [Header("Erosion")]
        [Range(0f, 1f)]
        public float ErosionStrength;

        [Header("Snowline")]
        [Range(0f, 1f)]
        public float Snowline;
        public string SnowTagID;

        [Header("Cave Low Freq Noise")]
        public string CaveLowFreqNoiseID;

        [Header("Cave High Freq Noise")]
        public string CaveHighFreqNoiseID;
        [Range(0f, 1f)]
        public float CaveThreshold;

        [Header("Ores")]
        public string OreNoiseID;
        [Range(0f, 1f)]
        public float OreThreshold;

        [Header("Stitch")]
        public int StitchPriority;
        public string StitchNoiseID;
        [Range(0f, 1f)]
        public float StitchStrength;

        [Header("Plants")]
        public bool GeneratePlants;
        public PlantGen[] PlantsToGenerate;
        public string PlantNoiseID;

        public PlantGen GetPlantGen(int i)
        {
            if (PlantsToGenerate.Length <= i) { return new PlantGen(); }
            return PlantsToGenerate[i];
        }

#if ODD_REALM_APP
        public void OnLoaded()
        {
            if (!string.IsNullOrEmpty(NoiseID))
            {
                TerrainNoise = DataManager.GetTagObject<GDEBiomeNoiseData>(NoiseID);
                CaveLowFreqNoise = DataManager.GetTagObject<GDEBiomeNoiseData>(CaveLowFreqNoiseID);
                CaveHighFreqNoise = DataManager.GetTagObject<GDEBiomeNoiseData>(CaveHighFreqNoiseID);
                OreNoise = DataManager.GetTagObject<GDEBiomeNoiseData>(OreNoiseID);
                PlantNoise = DataManager.GetTagObject<GDEBiomeNoiseData>(PlantNoiseID);
                StitchNoise = DataManager.GetTagObject<GDEBiomeNoiseData>(StitchNoiseID);
            }

            // Plant Tags
            for (int i = 0; i < PlantsToGenerate.Length; i++)
            {
                PlantsToGenerate[i].TagUID = DataManager.GetTagObject<GDETagsData>(PlantsToGenerate[i].TagID).TagUID;
            }

            // Cave Prop Tags
            //for (int i = 0; i < CavePropsToGenerate.Length; i++)
            //{
            //	CavePropsToGenerate[i].TagUID = DataManager.GetTagObject<GDETagsData>(CavePropsToGenerate[i].TagID).TagUID;
            //}

            SnowTagUID = DataManager.GetTagObject<GDETagsData>(SnowTagID).TagUID;

            RebuildSpread();
        }

        public void RebuildSpread()
        {
            if (SpreadCaveLayers == null || SpreadCaveLayers.Length != 256)
            {
                SpreadCaveLayers = new CaveLayer[256];
            }
            else
            {
                System.Array.Clear(SpreadCaveLayers, 0, SpreadCaveLayers.Length);
            }

            for (int i = 0; Caves != null && i < Caves.Length; i++)
            {
                int maxZ = (int)((Caves[i].Max + float.Epsilon) * 255);
                int minZ = (int)((Caves[i].Min + float.Epsilon) * 255);

                if (Caves[i].InteriorSpawns == null ||
                    Caves[i].InteriorSpawns.Length != Caves[i].InteriorSpawnTags.Length)
                {
                    Caves[i].InteriorSpawns = new GDECaveData[Caves[i].InteriorSpawnTags.Length];
                }

                for (int n = 0; n < Caves[i].InteriorSpawnTags.Length; n++)
                {
                    Caves[i].InteriorSpawns[n] = DataManager.GetTagObject<GDECaveData>(Caves[i].InteriorSpawnTags[n]);
                }

                for (int z = minZ; z <= maxZ; z++)
                {
                    SpreadCaveLayers[z] = Caves[i];
                }
            }

            // Null caves.
            for (int i = 0; i < SpreadCaveLayers.Length; i++)
            {
                if (SpreadCaveLayers[i] == null)
                {
                    SpreadCaveLayers[i] = CaveLayer.NULL;
                }
            }

            // Dungeon Layers.
            if (SpreadDungeonLayers == null || SpreadDungeonLayers.Length != 256)
            {
                SpreadDungeonLayers = new DungeonLayer[256];
            }
            else
            {
                System.Array.Clear(SpreadDungeonLayers, 0, SpreadDungeonLayers.Length);
            }

            for (int i = 0; Dungeons != null && i < Dungeons.Length; i++)
            {
                int maxZ = (int)((Dungeons[i].Max + float.Epsilon) * 255);
                int minZ = (int)((Dungeons[i].Min + float.Epsilon) * 255);

                if (Dungeons[i].DungeonsData == null ||
                    Dungeons[i].DungeonsData.Length != Dungeons[i].DungeonIDs.Length)
                {
                    Dungeons[i].DungeonsData = new GDEDungeonData[Dungeons[i].DungeonIDs.Length];
                }

                for (int n = 0; n < Dungeons[i].DungeonIDs.Length; n++)
                {
                    Dungeons[i].DungeonsData[n] = DataManager.GetTagObject<GDEDungeonData>(Dungeons[i].DungeonIDs[n]);
                }

                for (int z = minZ; z <= maxZ; z++)
                {
                    SpreadDungeonLayers[z] = Dungeons[i];
                }
            }

            // Null dungeons.
            for (int i = 0; i < SpreadDungeonLayers.Length; i++)
            {
                if (SpreadDungeonLayers[i] == null)
                {
                    SpreadDungeonLayers[i] = DungeonLayer.NULL;
                }
            }

            if (SpreadOreLayers == null || SpreadOreLayers.Length != 256)
            {
                SpreadOreLayers = new OreLayer[256];
            }
            else
            {
                System.Array.Clear(SpreadOreLayers, 0, SpreadOreLayers.Length);
            }

            for (int i = 0; Ores != null && i < Ores.Length; i++)
            {
                // int maxZ = (int)((Ores[i].Max + float.Epsilon) * 255);
                // int minZ = (int)((Ores[i].Min + float.Epsilon) * 255);
                int minZ = Mathf.CeilToInt(Ores[i].Min * 255f);
                int maxZ = Mathf.FloorToInt(Ores[i].Max * 255f);

                Ores[i].TagUIDs = new TagUID[Ores[i].TagIDs.Length];

                for (int n = 0; n < Ores[i].TagIDs.Length; n++)
                {
                    Ores[i].TagUIDs[n] = DataManager.GetTagObject<GDETagsData>(Ores[i].TagIDs[n]).TagUID;
                }

                for (int z = minZ; z <= maxZ; z++)
                {
                    SpreadOreLayers[z] = Ores[i];
                }
            }

            // Terrain.
            if (SpreadTerrainLayers == null || SpreadTerrainLayers.Length != 256)
            {
                SpreadTerrainLayers = new TerrainLayer[256];
            }
            else
            {
                System.Array.Clear(SpreadTerrainLayers, 0, SpreadTerrainLayers.Length);
            }

            SpreadTerrainLayers[0] = new TerrainLayer()
            {
                TagID = "tag_terrain_gen_bedrock",
                TagUID = DataManager.GetTagObject<GDETagsData>("tag_terrain_gen_bedrock").TagUID
            };

            for (int i = 0; Layers != null && i < Layers.Length; i++)
            {
                int maxZ = (int)((Layers[i].Max + float.Epsilon) * 255);

                if (!string.IsNullOrEmpty(Layers[i].TagID))
                {
                    Layers[i].TagUID = DataManager.GetTagObject<GDETagsData>(Layers[i].TagID).TagUID;
                }

                // start at 1 because 0 is bedrock
                for (int z = 1; z <= maxZ; z++)
                {
                    SpreadTerrainLayers[z] = Layers[i];
                }
            }
        }
#endif
    }

    [System.Serializable]
    public class PlantGen
    {
        [Header("TagID - (i.e., \"tag_spawns_biome_plants_barrens\")")]
        public string TagID;
        [HideInInspector]
        public TagUID TagUID;
        public float PlantThreshold;
        public float PlantCeiling;
        public float PlantFloor;
        public bool IsNULL { get { return string.IsNullOrEmpty(TagID); } }
    }

    public TerrainGen TerrainGenSettings = new();
    public DefaultPopulationRating[] DefaultFloraPopulationModifiers = new DefaultPopulationRating[0];
    public string OverworldMapGeneration = "";
    public string OverworldUnderlayVisuals = "overworld_visuals_ocean";
    public string OverworldTerrainVisuals = "";
    public string OverworldFeatureVisuals = "";
    public string OverworldRiverVisuals = "";
    public string OverworldRoadVisuals = "overworld_visuals_road_default";
    public float MovementCost = 1.0f;
    public float MovementSpeed = 0.2f;
    public string GenerateNameKey = "";
    public string Travel = "";

    public Gradient DayNightColor;
    public Gradient DayNightCloudColor;

    public List<string> Seasons = new();

    public List<string> SurfaceHostiles = new();

    [System.NonSerialized]
    public List<ITagObject> LocalFauna = new();

    [System.NonSerialized]
    public List<ITagObject> LocalFish = new();

    [System.NonSerialized]
    public List<ITagObject> LocalFlora = new();

#if ODD_REALM_APP
    public override void OnLoaded()
    {
#if DEV_TESTING
        for (int i = 0; i < SurfaceHostiles.Count; i++)
        {
            if (!DataManager.TagObjectExists(SurfaceHostiles[i]))
            {
                Debug.LogError($"Biome {name} has a missing surface hostile tag object: {SurfaceHostiles[i]}");
            }
        }
#endif

        if (DEBUG)
        {
            // Breakpoint.
        }

        TerrainGenSettings.OnLoaded();

        List<ITagObject> entities = DataManager.GetTagObjects<GDEEntitiesData>();
        List<ITagObject> fish = DataManager.GetTagObjects<GDEFishData>();

        LocalFauna.Clear();

        // Entities.
        for (int i = 0; i < entities.Count; i++)
        {
            GDEEntitiesData entityData = entities[i] as GDEEntitiesData;

            if (!entityData.BiomesHash.Contains(Key)) { continue; }

            LocalFauna.Add(entities[i]);
        }

        LocalFish.Clear();

        // Fish.
        for (int i = 0; i < fish.Count; i++)
        {
            GDEFishData fishData = fish[i] as GDEFishData;

            if (!fishData.BiomesHash.Contains(Key)) { continue; }

            LocalFish.Add(fish[i]);
        }

        // Flora.
        for (int i = 0; i < TerrainGenSettings.PlantsToGenerate.Length; i++)
        {
            ITag plantSpawnTag = DataManager.GetTagData(TerrainGenSettings.PlantsToGenerate[i].TagID);
            List<ITagObject> tagObjsByTag = DataManager.GetTagObjectsByTag(plantSpawnTag);

            for (int n = 0; n < tagObjsByTag.Count; n++)
            {
                GDETagObjectSpawnData spawn = DataManager.GetTagObject<GDETagObjectSpawnData>(tagObjsByTag[n].Key);

                if (spawn != null)
                {
                    for (int t = 0; t < spawn.Spawns.Length; t++)
                    {
                        ITag spawnTag = DataManager.GetTagData(spawn.Spawns[t].TagID);

                        List<ITagObject> spawnTagObjs = DataManager.GetTagObjectsByTag(spawnTag);

                        for (int s = 0; s < spawnTagObjs.Count; s++)
                        {
                            ITagObject spawnTagObj = spawnTagObjs[s];

                            // Flora.
                            if (spawnTagObj is GDEBlockPlantsData plantData)
                            {
                                if (!LocalFlora.Contains(spawnTagObj))
                                {
                                    LocalFlora.Add(spawnTagObj);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError(tagObjsByTag[n].Key + " is not a spawn data object!");
                }
            }
        }

        base.OnLoaded();
    }

    private const float VERY_RARE = 0.05f;
    private const float RARE = 0.15f;
    private const float UNCOMMON = 0.5f;
    private const float COMMON = 0.666f;
    private const float VERY_COMMON = 1.0f;

    public static PopulationRatings GetRatingFromNormal(float n)
    {
        if (n < VERY_RARE)
        {
            return PopulationRatings.VERY_RARE;
        }
        else if (n < RARE)
        {
            return PopulationRatings.RARE;
        }
        else if (n < UNCOMMON)
        {
            return PopulationRatings.UNCOMMON;
        }
        else if (n < COMMON)
        {
            return PopulationRatings.COMMON;
        }
        else
        {
            return PopulationRatings.VERY_COMMON;
        }
    }

    public static float GetRandomRatingNormal(PopulationRatings rating, int seed)
    {
        switch (rating)
        {
            case PopulationRatings.VERY_RARE:
                return TinyBeast.Random.FloatRange(0f, VERY_RARE, seed);

            case PopulationRatings.RARE:
                return TinyBeast.Random.FloatRange(VERY_RARE, RARE, seed);

            case PopulationRatings.UNCOMMON:
                return TinyBeast.Random.FloatRange(RARE, UNCOMMON, seed);

            case PopulationRatings.COMMON:
                return TinyBeast.Random.FloatRange(UNCOMMON, COMMON, seed);

            case PopulationRatings.VERY_COMMON:
            default:
                return TinyBeast.Random.FloatRange(COMMON, VERY_COMMON, seed);
        }
    }

#endif
}
