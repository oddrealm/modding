using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntitySizes")]
public class GDEEntitySizesData : ScriptableObject
{
	public string Key;
	public string FriendlyName = "";
	public int SizeType = 0;
}
