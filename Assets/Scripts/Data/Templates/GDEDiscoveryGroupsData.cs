using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DiscoveryGroups")]
public class GDEDiscoveryGroupsData : Scriptable
{
    public List<string> DiscoveryIDs = new List<string>();
}
