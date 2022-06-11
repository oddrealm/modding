using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FishSpawnGroups")]
public class GDEFishSpawnGroupsData : ScriptableObject
{
	public string Key { get { return name; } }
	public string Data = "";
}
