using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BlockTriggers")]
public class GDEBlockTriggersData : ScriptableObject
{
	public string Key;
	public string SpawnTag = "";
}
