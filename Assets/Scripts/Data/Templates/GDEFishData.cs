using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Fish")]
public class GDEFishData : Scriptable, IProgressionObject
{
    public int Level = 0;
    [Header("Max count in 128x128 world. Bigger worlds will factor up.")]
    public int MaxNaturalSpawnCount = 8;
    public string AnimController = "";
    public int IdealDepthMin = 0;
    public int IdealDepthmax = 0;
    public int BaseHealth = 0;
    public int BaseToughness = 0;
    public int BaseEvasion = 0;
    public string ItemSpawnGroupID = "";
    public Color FishColorR;
    public Color FishColorG;
    public Color FishColorB;

    public List<string> Biomes = new();
    public HashSet<string> BiomesHash = new();

    public bool CanShowInProgressUI { get { return true; } }
    public GDETagsData FishTagData { get; private set; }

    public override bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking()
        {
            TagID = "tag_fish",
            TagObjectID = Key,
            HideIfZero = true,
            TrackingType = TrackingTypes.FISH
        };

        return true;
    }

#if ODD_REALM_APP
    public override void Init()
    {
        BiomesHash.Clear();

        for (int i = 0; i < Biomes.Count; i++)
        {
            BiomesHash.Add(Biomes[i]);
        }

        base.Init();
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        EnsureTag("tag_fish");
        FishTagData = DataManager.GetTagObject<GDETagsData>("tag_fish");
    }
#endif
}
