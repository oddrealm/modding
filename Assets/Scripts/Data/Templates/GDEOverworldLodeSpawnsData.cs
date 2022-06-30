using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldLodeSpawns")]
public class GDEOverworldLodeSpawnsData : ScriptableObject
{
	public string Key;
	public int Shape = 0;
	public int SpawnCount = 0;
	public int MinZ = 0;
	public int MaxZ = 0;
	public List<string> Composition = new List<string>();
}
