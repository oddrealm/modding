using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Races")]
public class GDERacesData : Scriptable, IProgressionObject
{
    public int MerchantValue = 0;
    public string Intelligence;
    public List<string> Tags = new();
    public string DefaultEntityID = "";
    public bool Public = false;
    public List<string> Perks = new();
    public List<string> DefaultActiveResearch = new();
    public RandomEquipmentItem[] StartingItems = System.Array.Empty<RandomEquipmentItem>();
    public string[] Statuses = System.Array.Empty<string>();
    public BuffData[] Buffs = System.Array.Empty<BuffData>();
    public bool AutoCreateUniform = true;

    public bool CanShowInProgressUI
    {
        get
        {
            return true;
        }
    }

    public override bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking()
        {
            TagID = "tag_races",
            TagObjectID = Key,
            HideIfZero = true,
            TrackingType = TrackingTypes.ENTITY
        };

        return true;
    }

#if ODD_REALM_APP
    public override void Init()
    {
        base.Init();
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        EnsureTag("tag_races");
    }
#endif
}
