using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityBuffGroups")]
public class GDEEntityBuffGroupsData : ScriptableObject
{
	public string Key { get { return name; } }
	public List<string> Buffs = new List<string>();
}
