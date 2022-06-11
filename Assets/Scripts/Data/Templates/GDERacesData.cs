using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Races")]
public class GDERacesData : ScriptableObject
{
	public string Key { get { return name; } }
	public string TooltipID = "";
	public int MerchantValue = 0;
	public int IntelligenceType = 0;
	public List<string> Tags = new List<string>();
	public List<string> StartingResearchKeys = new List<string>();
	public string DefaultEntityID = "";
	public string Diet = "";
	public bool Public = false;
	public string BuffGroupID = "";
	public List<string> Perks = new List<string>();
}
