using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Indications")]
public class GDEIndicationsData : ScriptableObject
{
	public string Key { get { return name; } }
	public int Priority = 0;
	public string IconID = "";
	public float Duration = 0.0f;
	public float Interval = 0.0f;
	public bool Loop = false;
	public string MoveCurve = "";
	public string FadeCurve = "";
	public string ColorCurve = "";
}
