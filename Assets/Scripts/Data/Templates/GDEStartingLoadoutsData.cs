using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StartingLoadouts")]
public class GDEStartingLoadoutsData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public bool Public = false;
	public string EntitySpawnGroupID = "";
	public string Items = "";
	public int[] ContainerModels = new int[]
	{
		370, // wood barrel
		376 // wood create
	};
}
