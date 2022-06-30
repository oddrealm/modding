using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WordGroups")]
public class GDEWordGroupsData : ScriptableObject
{
	public string Key;
	public List<string> Words = new List<string>();
}
