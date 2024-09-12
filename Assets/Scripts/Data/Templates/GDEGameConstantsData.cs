using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameConstants")]
public class GDEGameConstantsData : Scriptable
{
	public string StringValue = "";
	public int IntValue = 0;
	public float FloatValue = 0.0f;
	public bool BoolValue = false;
	public List<string> StringListValue = new List<string>();
}
