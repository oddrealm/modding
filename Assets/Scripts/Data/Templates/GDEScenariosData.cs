using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Scenarios")]
public class GDEScenariosData : ScriptableObject
{
	public string Key;
	public int Index = 0;
	public int ActivationType = 0;
	public bool IsThreat = false;
	public string TooltipID = "";
	public string TileNoteTooltipID = "";
	public bool TryQueueOnNewGame = false;
	public int TileChance = 0;
	public int ActivationChance = 0;
	public int MinGameDays = 0;
	public int MinSettlerLevel = 0;
	public int MaxSettlerLevel = 0;
	public int MaxIDActivations = 0;
	public int MaxTagActivations = 0;
	public int IDInterval = 0;
	public int TagInterval = 0;
	public string Tag = "";
	public string RequiredTag = "";
	public List<string> RequiredEntityTagsInTile = new List<string>();
	public List<string> RequiredRoomFunctions = new List<string>();
	public List<string> PermittedNations = new List<string>();
	public List<int> PermittedWeather = new List<int>();
	public List<string> PermittedBiomes = new List<string>();
	public List<string> PermittedRaces = new List<string>();
	public List<string> PermittedSeasons = new List<string>();
	public string NotificationOnActivate = "";
	public string Components = "";
}
