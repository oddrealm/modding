using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Realm/History", order = 30)]
public class GDEHistoryData : Scriptable
{
    public bool CanBeRandomHistory = false;
    public int AutoPopulateTooltipPoolCount = 0;
    public EntityHistoryTypes HistoryType = EntityHistoryTypes.ENTITY_INTERACTION;
    public EntityIntelligenceTypes RestrictedIntelligenceTypes = EntityIntelligenceTypes.NON_SAPIENT;
    public List<string> RestrictedRaces = new List<string>();

}
