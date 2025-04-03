using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Block/BlockPlatforms", order = 23)]
public class GDEBlockPlatformsData : Scriptable
{
    public int Index = 0;
    public string Visuals = "";
    public float PhysicsHeatPass = 0.0f;
    public bool HasMeltThreshold = false;
    public int MeltThreshold = 0;
    public bool NeedsSupport = false;
    public BlockDirectionFlags PermittedPaths = BlockDirectionFlags.ALL & ~BlockDirectionFlags.DOWN;
    public BlockPermissionTypes Permissions = BlockPermissionTypes.NONE;
    public BlockPermissionTypes Prohibited = BlockPermissionTypes.NONE;
    public float MovementSpeedMod = 0.0f;
    public int MovementCost = -1;
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
