using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BiomeSeasons")]
public class GDEBiomeSeasonsData : Scriptable
{
	public string SeasonGroup = "spring";
	public int SeasonStart = 0;
	[Header("Determines weather sim for initial world load.")]
	public string DefaultWeather = "weather_default_snow";
	public List<string> Weather = new List<string>();
	public Color SeasonColor;
}
