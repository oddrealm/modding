using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityBuffTuning")]
public class GDEEntityBuffTuningData : ScriptableObject
{
	public string Key;
	public string BuffID = "";
	public int Amount = 0;
	public int BaseBuff = 0;
	public int MaxBuff = 0;
	public int Duration = 0;
	public bool StayActive = false;
}
