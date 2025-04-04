using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemSlots")]
public class GDEItemSlotsData : Scriptable
{
    public string EmptyIcon = "";
    public int GenerateItemChances = 1000;
    public int ListOrder = 0;
#if ODD_REALM_APP
    public override void SetOrderKey(string orderKey)
    {
        base.SetOrderKey(ListOrder.ToString());
    }
#endif
}
