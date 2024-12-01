using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Leader")]
public class GDELeaderData : Scriptable
{
    public string[] Statuses = new string[] { };

    public bool IsPassedToChildren;

    public string CompanionRole;

    public string[] Requirements;
}
