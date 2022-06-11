using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DiscoveryGroups")]
public class GDEDiscoveryGroupsData : ScriptableObject
{
	public string Key { get { return name; } }
	public List<string> DiscoveryIDs = new List<string>();
}
