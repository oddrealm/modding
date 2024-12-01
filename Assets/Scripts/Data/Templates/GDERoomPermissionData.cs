using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomPermission")]
public class GDERoomPermissionData : Scriptable
{
    public List<string> StatusTypes = new List<string>();
    public List<string> TagTypes = new List<string>();
}
