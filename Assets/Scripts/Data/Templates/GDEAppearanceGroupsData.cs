using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AppearanceGroups")]
public class GDEAppearanceGroupsData : ScriptableObject
{
	public string Key { get { return name; } }
	public List<string> AppearanceIDs = new List<string>();
}
