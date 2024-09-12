using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Intelligence")]
public class GDEIntelligenceData : Scriptable
{
	public EntityIntelligenceTypes IntelligenceType = EntityIntelligenceTypes.SAPIENT;
	public List<string> Statuses;
}
