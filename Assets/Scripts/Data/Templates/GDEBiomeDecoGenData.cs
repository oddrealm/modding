using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BiomeDecoGen")]
public class GDEBiomeDecoGenData : ScriptableObject
{
	public string Key;
	public int SpawnType = 0;
	public int ModelRequirement = 0;
	public List<int> ModelsToSet = new List<int>();
	public int ModelAboveRequirement = 0;
	public List<int> ModelsToSetAbove = new List<int>();
	public float NoiseThreshold = 0.0f;
	public float NoiseFrequency = 0.0f;
	public float MinZ = 0.0f;
	public float MaxZ = 0.0f;
	public int CountType = 0;
	public int MinCount = 0;
	public int MaxCount = 0;
}
