using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemSlots")]
public class GDEItemSlotsData : Scriptable
{
	public string EmptyIcon = "";
	public int GenerateItemChances = 1000;
}
