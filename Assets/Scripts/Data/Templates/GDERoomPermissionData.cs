using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Room/RoomPermission", order = 36)]
public class GDERoomPermissionData : Scriptable
{
    public List<string> StatusTypes = new List<string>();
    public List<string> TagTypes = new List<string>();
}
