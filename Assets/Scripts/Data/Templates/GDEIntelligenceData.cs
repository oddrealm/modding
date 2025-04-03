using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Kingdom/Intelligence", order = 27)]
public class GDEIntelligenceData : Scriptable
{
    public EntityIntelligenceTypes IntelligenceType = EntityIntelligenceTypes.SAPIENT;
    public List<string> Statuses;
}
