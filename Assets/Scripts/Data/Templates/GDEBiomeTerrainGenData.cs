using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BiomeTerrainGen")]
public class GDEBiomeTerrainGenData : ScriptableObject
{
	public string Key { get { return name; } }
	public int ModelIndex = 0;
	public string Noise = "";
}
