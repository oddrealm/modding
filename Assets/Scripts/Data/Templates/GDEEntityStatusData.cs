using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityStatus")]
public class GDEEntityStatusData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public bool HasEntityInfo = false;
	public bool ShowToPlayer = false;
	public string Indicator = "";
	public string Notification = "";
	public string BuffGroup = "";
	public string OverworldPartyActionGroup = "";
	public int ExpireTimeMinutesMin = 0;
	public int ExpireTimeMinutesMax = 0;
	public int ActivateIntervals = 0;
	public string ActivateItems = "";
	public List<string> StatusesToRemove = new List<string>();
	public List<string> StatusesToAddOnExpire = new List<string>();
	public List<string> PermittedGenders = new List<string>();
	public int CooldownMin = 0;
	public int CooldownMax = 0;
	public string EntitiesToSpawnOnExpire;
}
