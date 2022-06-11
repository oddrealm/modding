using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldNation")]
public class GDEOverworldNationData : ScriptableObject
{
	public string Key { get { return name; } }
	public int Order;
	public string TooltipID = "";
	public bool Unlocked = false;
	public string Banner = "";
	public string OverworldVisuals = "";
	public string Color = "";
	public List<string> Loadouts = new List<string>();
	public List<string> Races = new List<string>();
	public List<string> DiscoveredRaces = new List<string>();
	public string GenerateNameKey = "";
	public int OriginCount = 0;
	public int MinCount = 0;
	public int MaxCount = 0;
	public List<string> SettlementPrefabs = new List<string>();
	public List<string> Biomes = new List<string>();
	public List<string> HostileTowards = new List<string>();
	public List<string> FriendlyDiscoveryGroups = new List<string>();
	public List<string> HostileDiscoveryGroups = new List<string>();
}
