using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ModelPrefabs")]
public class GDEModelPrefabsData : ScriptableObject
{
	public string Key { get { return name; } }
	public string Group = "";
	public int Width = 0;
	public int Height = 0;
	public int Depth = 0;
	public List<string> Layout = new List<string>();
}
