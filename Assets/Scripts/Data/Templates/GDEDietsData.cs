using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Diets")]
public class GDEDietsData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public List<string> HarmfulFoods = new List<string>();
	public List<string> HatedFoods = new List<string>();
	public List<string> LovedFoods = new List<string>();
	public List<string> LowPriorityFoods = new List<string>();
	public List<string> MedPriorityFoods = new List<string>();
	public List<string> HighPriorityFoods = new List<string>();
}
