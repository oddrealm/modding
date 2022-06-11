using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CommandButtons")]
public class GDECommandButtonsData : ScriptableObject
{
	public string Key { get { return name; } }
	public List<string> DependencyTag = new List<string>();
	public int ActivationType = 0;
	public string ActiveTooltipName = "";
	public bool SaveBtnState = false;
	public string InactiveTooltipName = "";
	public string ActiveDisplayText = "";
	public string InactiveDisplayText = "";
	public string ActiveHotkeyDisplay = "";
	public string InactiveHotkeyDisplay = "";
	public string Hotkey = "";
	public string WindowNameToToggle = "";
	public string WindowLayerToToggle = "";
	public string Description = "";
}
