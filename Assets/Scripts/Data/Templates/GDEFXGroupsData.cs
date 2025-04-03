using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FXGroups", order = 0)]
public class GDEFXGroupsData : Scriptable
{
    public List<string> FX = new List<string>();
}
