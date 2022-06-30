using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Biomes")]
public class GDEBiomesData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public List<string> TerrainGeneration = new List<string>();
	public List<string> ModelPrefabGen = new List<string>();
	public List<string> DecoGeneration = new List<string>();
	public List<string> LodeGeneration = new List<string>();
	public string OverworldMapGeneration = "";
	public string OverworldTerrainVisuals = "";
	public string OverworldRiverVisuals = "";
	public float MovementCost = 0.0f;
	public string GenerateNameKey = "";
	public string Travel = "";
	public List<string> Fish = new List<string>();
	public List<string> Seasons = new List<string>();
}
