using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Zones", order = 0)]
public class GDEZonesData : Scriptable
{
    public string Visuals = "";
    public bool VisibleToPlayer = false;
    public int OrderIndex = 0;
    public ItemCategories ItemPermissions = 0;
    public bool ShowLowNotifByDefault = false;
    public bool HideIfZeroByDefault = false;
}
