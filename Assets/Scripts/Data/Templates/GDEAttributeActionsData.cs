using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AttributeActions")]
public class GDEAttributeActionsData : ScriptableObject
{
	public string Key { get { return name; } }
	public string FriendlyName = "";
	public int ConditionEvent = 0;
	public string Condition = "";
	public List<string> ActiveStatuses = new List<string>();
	public List<string> InactiveStatuses = new List<string>();
}
