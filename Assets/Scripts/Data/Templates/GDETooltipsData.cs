using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tooltips")]
public class GDETooltipsData : ScriptableObject
{
	public string Key { get { return name; } }
	public string Name = "";
	public string InlineIcon = "";
	public string Description = "";
	public string Action = "";
	public string Icon = "";
	public string Formatting = "";
	public Color TextColor = Color.white;
}
