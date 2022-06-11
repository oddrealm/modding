using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AudioGroups")]
public class GDEAudioGroupsData : ScriptableObject
{
	public string Key { get { return name; } }
	public List<string> SFX = new List<string>();
}
