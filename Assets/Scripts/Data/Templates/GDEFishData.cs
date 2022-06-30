using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Fish")]
public class GDEFishData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public int Level = 0;
	public string AnimController = "";
	public int IdealDepthMin = 0;
	public int IdealDepthmax = 0;
	public int LureRollAdd = 0;
	public int BaseHealth = 0;
	public int BaseToughness = 0;
	public int BaseEvasion = 0;
	public string ItemSpawnGroupID = "";
}
