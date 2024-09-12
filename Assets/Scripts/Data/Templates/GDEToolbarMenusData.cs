using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ToolbarMenus")]
public class GDEToolbarMenusData : Scriptable
{
	public int TabOrder = 0;
	public ToolbarToggleTypes ToggleType = ToolbarToggleTypes.BLUEPRINT;
	public SelectionTypes SelectionType = SelectionTypes.OBJECT;
	public string HotkeyID = "";
	public List<string> MenuList = new List<string>();
	public BlueprintCategories ButtonConstructionCategories = 0;
}
