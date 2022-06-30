using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Zones")]
public class GDEZonesData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public string Visuals = "";
	public bool VisibleToPlayer = false;
	public int OrderIndex = 0;
	public ItemCategories ItemPermissions = 0;
}
