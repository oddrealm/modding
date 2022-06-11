using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldTravel")]
public class GDEOverworldTravelData : ScriptableObject
{
	public string Key { get { return name; } }
	public string PeacefulActionGroup = "";
	public string UntamedActionGroup = "";
	public string FerociousActionGroup = "";
}
