using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Block/BlockModel", order = 21)]
public class GDEBlockModelData : Scriptable
{
	public int ModelIndex = 0;
	public int BlockIndex = -1;
	public bool ReplacePreviousPlants = false;
	public List<string> Plants = new List<string>();
	public string ItemSpawnGroup = "";
	public List<string> Items = new List<string>();
	public string Platform = "";
	public string ContainedEntitySpawnGroup = "";
	public string FishSpawnGroup = "";
	public bool AppearInPaint = true;
}
