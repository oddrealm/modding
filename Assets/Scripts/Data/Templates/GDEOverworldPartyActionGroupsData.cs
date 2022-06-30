using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldPartyActionGroups")]
public class GDEOverworldPartyActionGroupsData : ScriptableObject
{
	public string Key;
	public List<string> Actions = new List<string>();
}
