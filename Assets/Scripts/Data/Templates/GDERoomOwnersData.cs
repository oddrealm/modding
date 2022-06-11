using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomOwners")]
public class GDERoomOwnersData : ScriptableObject
{
	public string Key { get { return name; } }
	public string TooltipID = "";
	public bool LimitToUnique = false;
	public bool LimitByFamily = false;
	public int MaxOwners = -1;
	public EntityIntelligenceTypes PermittedSapientTypes = EntityIntelligenceTypes.SAPIENT;
	public List<string> DefaultRaces = new List<string>();
	public List<string> DefaultSkills = new List<string>();
	public List<string> DefaultProfessions = new List<string>();
	public List<string> DefaultFactions = new List<string>()
	{
		"faction_player"
	};
}
