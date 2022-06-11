using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomStockpiles")]
public class GDERoomStockpilesData : ScriptableObject
{
	public string Key { get { return name; } }
	public string TooltipID = "";
	public List<string> Items = new List<string>();
	public List<string> Categories = new List<string>();
}
