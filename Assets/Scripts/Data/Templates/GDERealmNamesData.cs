using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RealmNames")]
public class GDERealmNamesData : ScriptableObject
{
	public string Key { get { return name; } }
	public List<string> FrontCompounds = new List<string>();
	public List<string> RearCompounds = new List<string>();
}
