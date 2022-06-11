using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityStatus")]
public class GDEEntityStatusData : ScriptableObject
{
	public string Key { get { return name; } }
	public string TooltipID = "";
	public string Tag = "";
	public bool ShowToPlayer = false;
	public string Indicator = "";
	public string Notification = "";
	public string BuffGroup = "";
	public string OverworldPartyActionGroup = "";
	public int ExpireTimeMinutes = 0;
	public int ActivateIntervals = 0;
	public string ActivateItems = "";
}
