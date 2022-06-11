using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AttackGroups")]
public class GDEAttackGroupsData : ScriptableObject
{
	public string Key { get { return name; } }
	public List<string> Attacks = new List<string>();
}
