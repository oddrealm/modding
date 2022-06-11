using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldPartyActions")]
public class GDEOverworldPartyActionsData : ScriptableObject
{
	public string Key { get { return name; } }
	public int Chances = 0;
	public string TooltipID = "";
	public int ActivationType = 0;
	public int Actions = 0;
	public string ItemSpawnGroup = "";
	public string EntitySpawnGroup = "";
	public string ActivateFX = "";
	public string DialogueID = "";
}
