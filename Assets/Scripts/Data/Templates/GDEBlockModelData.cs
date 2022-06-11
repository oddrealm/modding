using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BlockModel")]
public class GDEBlockModelData : ScriptableObject
{
	public string Key { get { return name; } }
	public int ModelIndex = 0;
	public int BlockIndex = -1;
	public List<string> Plants = new List<string>();
	public string ItemSpawnGroup = "";
	public List<string> Items = new List<string>();
	public string Platform = "";
	public string ContainedEntitySpawnGroup = "";
	public string FishSpawnGroup = "";
	public bool AppearInPaint = true;
}
