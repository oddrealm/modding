using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LocationNames")]
public class GDELocationNamesData : ScriptableObject
{
	public string Key;
	public List<string> Determiner = new List<string>();
	public List<string> Prepends = new List<string>();
	public List<string> Names = new List<string>();
}
