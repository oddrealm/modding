using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueGroups")]
public class GDEDialogueGroupsData : ScriptableObject
{
	public string Key { get { return name; } }
	public List<string> DialogueIDs = new List<string>();
}
