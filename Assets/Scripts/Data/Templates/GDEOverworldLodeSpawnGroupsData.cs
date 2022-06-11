using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldLodeSpawnGroups")]
public class GDEOverworldLodeSpawnGroupsData : ScriptableObject
{
	public string Key { get { return name; } }
	public List<string> Spawns = new List<string>();
}
