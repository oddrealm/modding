using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BlockPlatforms")]
public class GDEBlockPlatformsData : ScriptableObject
{
	public string Key;
	public int Index = 0;
	public string Visuals = "";
	public string TooltipID = "";
	public float PhysicsHeatPass = 0.0f;
	public bool HasMeltThreshold = false;
	public int MeltThreshold = 0;
	public bool NeedsSupport = false;
	public BlockPermissionTypes Permissions = 0;
	public float MovementSpeedMod = 0.0f;
	public float MovementCostMult = 0.0f;
	public bool IsRoof = false;
	public bool CanGrow = false;
	public int GrowTime = 0;
	public int GrowLightReq = 0;
	public bool BlocksCursorRaycast = false;
	public bool BlocksVerticalLight = false;
	public int VerticalLightPass = 0;
	public bool CanWorkThrough = false;
	public bool CanBuildOnWater = false;
	public int FlammableChance = 0;
	public int BurnFuel = 0;
	public int BaseHealth = 0;
	public int BaseToughness = 0;
	public string ItemDropGroupID = "";
}
