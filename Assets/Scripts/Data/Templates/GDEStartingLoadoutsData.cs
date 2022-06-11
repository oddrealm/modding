using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StartingLoadouts")]
public class GDEStartingLoadoutsData : ScriptableObject
{
	public string Key { get { return name; } }
	public string TooltipID = "";
	public bool Public = false;
	public string EntitySpawnGroupID = "";
	public string Items = "";
}
