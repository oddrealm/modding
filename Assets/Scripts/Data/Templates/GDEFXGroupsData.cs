using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FXGroups")]
public class GDEFXGroupsData : ScriptableObject
{
	public string Key;
	public List<string> FX = new List<string>();
}
