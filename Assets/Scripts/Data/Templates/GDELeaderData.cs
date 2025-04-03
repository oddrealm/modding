using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Kingdom/Leader", order = 28)]
public class GDELeaderData : Scriptable
{
    public string[] Statuses = new string[] { };

    public bool IsPassedToChildren;

    public string CompanionRole;

    public string[] Requirements;
}
