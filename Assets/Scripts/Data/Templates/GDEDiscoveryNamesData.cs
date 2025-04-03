using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Event/DiscoveryNames", order = 18)]
public class GDEDiscoveryNamesData : Scriptable
{
    public List<string> Determiner = new List<string>();
    public List<string> Prepends = new List<string>();
    public List<string> Names = new List<string>();
}
