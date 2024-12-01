using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueGroups")]
public class GDEDialogueGroupsData : Scriptable
{
    public List<string> DialogueIDs = new List<string>();
}
