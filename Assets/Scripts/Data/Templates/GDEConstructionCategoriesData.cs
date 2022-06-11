using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ConstructionCategories")]
public class GDEConstructionCategoriesData : ScriptableObject
{
	public string Key { get { return name; } }
	public string MenuKey = "";
	public int MenuIndex = 0;
	public string Name = "";
	public string Description = "";
	public bool ShowInBuildMenu = false;
	public bool ShowInPlantMenu = false;
	public bool ShowInBlueprintsOverlay = false;
	public bool ShowInWorkstationsOverlay = false;
	public string InlineIcon = "";
	public BlueprintCategories CategoryType = 0;
}
