using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Realm/LocationNames", order = 32)]
public class GDELocationNamesData : Scriptable
{
    public List<string> Determiner = new List<string>();
    public List<string> Prepends = new List<string>();
    public bool SpaceAfterPrepend = false;
    public List<string> Names = new List<string>();
    public bool SpaceBeforeAppend = false;
    public List<string> Appends = new List<string>();
}
