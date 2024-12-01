using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AppearanceGroups")]
public class GDEAppearanceGroupsData : Scriptable
{
    public List<string> AppearanceIDs = new List<string>();
}
