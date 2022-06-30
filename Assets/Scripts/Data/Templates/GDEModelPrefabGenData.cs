using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ModelPrefabGen")]
public class GDEModelPrefabGenData : ScriptableObject
{
	public string Key;
	public bool GenerateCorridors = false;
	public int PlacementType = 0;
	public int ModelRequirement = 0;
	public int ModelRequirementBelow = 0;
	public float NoiseThreshold = 0.0f;
	public float NoiseFrequency = 0.0f;
	public int RandomChance = 0;
	public int MaxOccurences = 0;
	public int AdditionalAttemptsOnFail = 0;
	public bool UseMapSizeToOffsetPrefabCount = false;
	public int PrefabCountMin = 0;
	public int PrefabCountMax = 0;
	public float MinZLevel = 0.0f;
	public float MaxZLevel = 0.0f;
	public int MaxLight = 0;
	public float MaxSpreadMod = 0.0f;
	public string ModelPrefabGroup = "";
	public string ModelPrefabSpecGroup = "";
}
