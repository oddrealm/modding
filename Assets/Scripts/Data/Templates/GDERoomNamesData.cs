using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Room/RoomNames", order = 35)]
public class GDERoomNamesData : Scriptable
{
    public List<string> FrontCompounds = new List<string>();
    public List<string> MiddleCompounds = new List<string>();
    public List<string> RearCompounds = new List<string>();
}
