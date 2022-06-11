using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue")]
public class GDEDialogueData : ScriptableObject
{
	public string Key { get { return name; } }
	public string AppearanceID = "";
	public List<string> NextDialogueID = new List<string>();
	public string SpeakerTarget = "";
	public string Message = "";
	public List<string> Response = new List<string>();
}
