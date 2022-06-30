using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BlockPlants")]
public class GDEBlockPlantsData : ScriptableObject
{
	public string Key;
	public int Index = 0;
	public bool IsRoot = false;
	public List<string> Biomes = new List<string>();
	public List<string> Seasons = new List<string>();
	public int NaturalSpawnBlock = 0;
	public BlockPermissionTypes Permissions = 0;
	public BlockPermissionTypes MaturePermissions = 0;
	public BlockPlantTypes PlantType = 0;
	public BlockPlantTypes PlantPermission = 0;
	public int Group = 0;
	public bool IsObstruction = false;
	public int FertilityRequirement = 0;
	public BlockPhysicsTypes PhysicsType = 0;
	public int NaturalSpawnChance = 0;
	public int NaturalReproduceChance = 0;
	public string ReproducePlant = "";
	public int ReproduceMinDist = 0;
	public int ReproduceMaxDist = 0;
	public string ReproduceFX = "";
	public int ReproduceIntervals = 0;
	public string TooltipID = "";
	public List<string> Visuals = new List<string>();
	public string RemoveItemDrops = "";
	public string MatureItemDrops = "";
	public string MatureEntityDrops = "";
	public string MaturePrefabID = "";
	public bool NeedsSkylight = false;
	public int MinTemp = 0;
	public int MaxTemp = 0;
	public int MatureTime = 0;
	public int MaxLifeTime = 0;
	public int GrowthStages = 0;
	public int FlammableChance = 0;
	public int HealthMax = 100;
	public string HealthTooltipID = "tooltip_attribute_health";
	public int MaintenanceDecayMin = 1;
	public int MaintenanceDecayMax = 2;
	public int MaintenanceMax = 100;
	public string MaintenanceTooltipID = "";
	public List<string> MaintenanceTags = new List<string>()
    {
		"needs_watering"
	};
	public List<string> MatureTags = new List<string>()
	{
		"mature_plant"
	};
}
