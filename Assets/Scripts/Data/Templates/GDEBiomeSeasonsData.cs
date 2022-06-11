using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BiomeSeasons")]
public class GDEBiomeSeasonsData : ScriptableObject
{
	public string Key { get { return name; } }
	public string TooltipID = "";
	public int SeasonType = 0;
	public int SeasonStart = 0;
	public List<string> Weather = new List<string>();
	public Color SeasonColor;
}
