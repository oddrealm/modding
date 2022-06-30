using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomNames")]
public class GDERoomNamesData : ScriptableObject
{
	public string Key;
	public List<string> FrontCompounds = new List<string>();
	public List<string> MiddleCompounds = new List<string>();
	public List<string> RearCompounds = new List<string>();
}
