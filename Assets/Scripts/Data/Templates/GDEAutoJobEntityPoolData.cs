using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AutoJobEntityPool")]
public class GDEAutoJobEntityPoolData : Scriptable
{
	public AutoJobEntityPoolTypes PoolType = AutoJobEntityPoolTypes.NONE;
}