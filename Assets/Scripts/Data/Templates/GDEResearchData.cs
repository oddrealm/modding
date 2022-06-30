using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Research")]
public class GDEResearchData : ScriptableObject
{
	public string Key;
	public bool Enabled = false;
	public List<string> Races = new List<string>();
	public string ResearchKey = "";
	public string TooltipID = "";
	public string Dependency = "";
	public int Column = 0;
	public int Row = 0;
	public string RequiredItem = "";
	public int RequiredItemCount = 0;
	public ResearchCategories ResearchCategory = 0;
}
