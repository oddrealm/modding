using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityBuffGroups")]
public class GDEEntityBuffGroupsData : ScriptableObject
{
	public string Key;
	public List<string> Buffs = new List<string>();
}
