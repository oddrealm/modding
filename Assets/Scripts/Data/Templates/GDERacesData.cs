using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Races")]
public class GDERacesData : Scriptable, IProgressionObject
{
    public int MerchantValue = 0;
    public string Intelligence;
    public List<string> Tags = new List<string>();
    public string DefaultEntityID = "";
    public bool Public = false;
    public List<string> Perks = new List<string>();
    public List<string> DefaultActiveResearch = new List<string>();
    public RandomEquipmentItem[] StartingItems = new RandomEquipmentItem[] { };
    public BuffData[] Buffs;
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
}
