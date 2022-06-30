using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomTemplates")]
public class GDERoomTemplatesData : ScriptableObject
{
	public string Key;
	public bool UpdateEveryFrame = false;
	public RoomActivationTypes ActivationType = RoomActivationTypes.DESIGNATION;
	public bool CanEditOwnersAndStockpiles = true;
	public bool Enabled = false;
	public string Stockpile = "";
	public string ResearchKey = "";
	public List<string> Owners = new List<string>();
	public List<string> DefaultAutoJobBlueprints = new List<string>();
	public string InterfaceInfo = "";
	public string TooltipID = "";
	public List<string> Visuals = new List<string>();
}
