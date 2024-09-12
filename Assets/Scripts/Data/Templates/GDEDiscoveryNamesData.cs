using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DiscoveryNames")]
public class GDEDiscoveryNamesData : Scriptable
{
	public List<string> Determiner = new List<string>();
	public List<string> Prepends = new List<string>();
	public List<string> Names = new List<string>();
}
