using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomStockpiles")]
public class GDERoomStockpilesData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public StoragePreferences StoragePreference = StoragePreferences.ALL;
	public List<string> Items = new List<string>();
	public List<ItemCategories> Categories = new List<ItemCategories>();
}
