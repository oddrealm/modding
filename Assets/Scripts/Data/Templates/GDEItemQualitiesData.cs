using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Item/ItemQualities", order = 22)]
public class GDEItemQualitiesData : Scriptable
{
    public string Group = "";
    public string FriendlyName = "";
    public int Quality = 0;
    public int ChanceToDegrade = 0;
    public string BuffGroupID = "";
}
