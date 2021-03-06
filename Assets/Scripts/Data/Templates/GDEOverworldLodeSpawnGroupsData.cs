using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldLodeSpawnGroups")]
public class GDEOverworldLodeSpawnGroupsData : ScriptableObject
{
	public string Key;
	public List<string> Spawns = new List<string>();
}
