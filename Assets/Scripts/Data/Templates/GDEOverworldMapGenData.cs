using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldMapGen")]
public class GDEOverworldMapGenData : Scriptable
{
	public int BuildPriority = 0;
	public int SeedOffset = 0;
	public float Frequency = 0.0f;
	public int Octaves = 0;
	public float Persistence = 0.0f;
	public float Lacunarity = 0.0f;
	public float MinThreshold = 0.0f;
	public float MaxThreshold = 0.0f;
	public float Falloff = 0.0f;
	public bool UseBotTopMinMax = false;
	[Header("If true, will measure dist from avg center of bot/top boundaries and fade based on that. False is a simple clamp.")]
	public bool FadeBotTopMinMax = true;
    public float TopMax = 0.0f;
	public float TopMin = 0.0f;
	public float BotMax = 0.0f;
	public float BotMin = 0.0f;
	public bool UseLongitudinalFade = false;
	public bool UseLateralFade = false;
	public bool InvertLongitudinalFade = false;
	public bool InvertLateralFade = false;
}
