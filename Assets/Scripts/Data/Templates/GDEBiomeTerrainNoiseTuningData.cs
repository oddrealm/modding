using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BiomeTerrainNoiseTuning")]
public class GDEBiomeTerrainNoiseTuningData : ScriptableObject
{
	public string Key { get { return name; } }
	public int Priority = 0;
	public float Frequency = 0.0f;
	public int Octaves = 0;
	public float Lacunarity = 0.0f;
	public float Persistence = 0.0f;
	public float Floor = 0.0f;
	public float Ceiling = 0.0f;
	public float ClampMin = 0.0f;
	public float ClampMax = 0.0f;
	public float Add = 0.0f;
	public bool HasPowFalloff = false;
	public float PowFalloff = 0.0f;
	public bool HasCoastFalloff = false;
	public float CoastFalloff = 0.0f;
}
