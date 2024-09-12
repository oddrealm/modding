using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/HistoryTypes")]
public class GDEHistoryTypesData : Scriptable
{
	public EntityHistoryTypes HistoryType = EntityHistoryTypes.ENTITY_INTERACTION;
}
