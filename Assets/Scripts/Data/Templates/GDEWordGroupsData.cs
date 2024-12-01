using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WordGroups")]
public class GDEWordGroupsData : Scriptable
{
    public List<string> Words = new List<string>();
}
