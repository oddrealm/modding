using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BiomeWeather")]
public class GDEBiomeWeatherData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public string UIDisplayAnimKey = "";
	public float FlammabilityMod = 0.0f;
	public int ChanceToSpawnSnow = 0;
	public int WeatherType = 0;
	public int NightOffset = 0;
	public int DayOffset = 0;
	public int MinTemp = 0;
	public int MaxTemp = 0;
	public int FXIndex = 0;
	public int ChanceToStart = 0;
	public int MinHourDuration = 0;
	public int HourDurationVariance = 0;
	public float Brightness = 0.0f;
	public string AquaticAmbientSFX = "";
	public string SubterraneanAmbientSFX = "";
	public string DaytimeAmbientSFX = "";
	public string NighttimeAmbientSFX = "";
}
