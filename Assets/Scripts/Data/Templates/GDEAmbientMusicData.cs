using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AmbientMusic")]
public class GDEAmbientMusicData : ScriptableObject
{
	public string Key;

	public string IntroID = "";
	public string LoopID = "";
	public int MinHour = 0;
	public int MaxHour = 24;
}
