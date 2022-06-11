using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BiomeNames")]
public class GDEBiomeNamesData : ScriptableObject
{
	public string Key { get { return name; } }
	public List<string> Determiner = new List<string>();
	public List<string> Names = new List<string>();
}
