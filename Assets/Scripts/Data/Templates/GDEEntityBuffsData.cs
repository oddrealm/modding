using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityBuffs")]
public class GDEEntityBuffsData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public EntityBuffTypes BuffType = EntityBuffTypes.ATTRIBUTES;
	public bool NegativeIsPositive = false;
	public bool AggregateSameType = false;
	public string Identifier = "";
}
