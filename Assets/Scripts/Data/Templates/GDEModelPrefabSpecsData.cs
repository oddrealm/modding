using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ModelPrefabSpecs")]
public class GDEModelPrefabSpecsData : ScriptableObject
{
	public string Key { get { return name; } }
	public string Group = "";
	public int ClearType = 0;
	public List<int> Corridor = new List<int>();
	public int CorridorType = 0;
	public List<int> Entrance = new List<int>();
	public int EntranceType = 0;
	public List<int> Vertical = new List<int>();
	public int VerticalType = 0;
	public List<int> Floor = new List<int>();
	public int FloorType = 0;
	public List<int> Roof = new List<int>();
	public int RoofType = 0;
	public List<int> A = new List<int>();
	public int AType = 0;
	public List<int> B = new List<int>();
	public int BType = 0;
	public List<int> C = new List<int>();
	public int CType = 0;
	public List<int> D = new List<int>();
	public int DType = 0;
}
