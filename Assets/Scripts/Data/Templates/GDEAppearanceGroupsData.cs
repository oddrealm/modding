using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AppearanceGroups")]
public class GDEAppearanceGroupsData : ScriptableObject
{
	public string Key;
	public List<string> AppearanceIDs = new List<string>();
}
