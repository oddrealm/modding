using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/XPGrowth")]
public class GDEXPGrowthData : ScriptableObject
{
	public string Key { get { return name; } }
	public int Level = 0;
	public int XPReward = 0;
	public int XPRequireToLevel = 0;
}
