using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/BlockPlatforms")]
public class GDEBlockPlatformsData : Scriptable, IProgressionObject
{
    public int RoomQualityAdd = 50;
    public int Index = 0;
    public bool IsFertile = false;
    public string Visuals = "";
    public float PhysicsHeatPass = 0.0f;
    public bool HasMeltThreshold = false;
    public int MeltThreshold = 0;
    public BlockDirectionFlags PermittedPaths = BlockDirectionFlags.ALL & ~BlockDirectionFlags.DOWN;
    public BlockPermissionTypes Permissions = BlockPermissionTypes.NONE;
    public BlockPermissionTypes Prohibited = BlockPermissionTypes.NONE;
    public float MovementSpeedMod = 0.0f;
    public int MovementCost = -1;
    public bool IsRoof = false;
    public bool CanGrowPlants = false;
    public bool AllowNaturalPlantGrowth = false;
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
    public string[] UsageTagIDs = System.Array.Empty<string>();

    public bool CanShowInProgressUI { get { return true; } }
    public GDETagsData[] UsageTags { get; private set; } = System.Array.Empty<GDETagsData>();
    public HashSet<string> UsageTagsHash { get; private set; } = new HashSet<string>();

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();
        EnsureTag("tag_platform");

        UsageTagsHash.Clear();
        UsageTags = new GDETagsData[UsageTagIDs.Length];

        for (int i = 0; i < UsageTagIDs.Length; i++)
        {
            UsageTags[i] = DataManager.GetTagObject<GDETagsData>(UsageTagIDs[i]);
            UsageTagsHash.Add(UsageTagIDs[i]);
        }
    }
#endif
}
