using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BiomeLodeGen")]
public class GDEBiomeLodeGenData : ScriptableObject
{
	public string Key { get { return name; } }
	public int ModelIndex = 0;
	public float NoiseThreshold = 0.0f;
	public float NoiseFrequency = 0.0f;
	public float MinZ = 0.0f;
	public float MaxZ = 0.0f;
	public int MinVeinSize = 0;
	public int MaxVeinSize = 0;
	public int MinCount = 0;
	public int MaxCount = 0;
}
