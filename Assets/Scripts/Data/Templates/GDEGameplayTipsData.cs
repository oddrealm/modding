using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameplayTips")]
public class GDEGameplayTipsData : ScriptableObject
{
	public string Key { get { return name; } }
	public string DisplayText = "";
	public string HotkeyID = "";
}
