using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory")]
public class GDEInventoryData : ScriptableObject
{
	public string Key { get { return name; } }
	public string FriendlyName = "";
	public bool FillAllSlots = false;
	public List<string> Slots = new List<string>();
	public string InlineIcon = "";
	public int InventoryType = 0;
}
