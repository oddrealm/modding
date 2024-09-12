using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemQualities")]
public class GDEItemQualitiesData : Scriptable
{
	public string Group = "";
	public string FriendlyName = "";
	public int Quality = 0;
	public int ChanceToDegrade = 0;
	public string BuffGroupID = "";
}
