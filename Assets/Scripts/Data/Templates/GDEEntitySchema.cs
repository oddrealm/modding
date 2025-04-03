using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Entity/EntitySchema", order = 21)]
public class GDEEntitySchema : Scriptable
{
    [System.Serializable]
    public class EquipmentLoadout
    {
        public string ItemID;
        public string SlotID;
    }

    [System.Serializable]
    public class InventoryLoadout
    {
        public string ItemID;
        public int ItemCount = 1;
    }

    [System.Serializable]
    public class AttributeOverride
    {
        public string Attribute;
        public int Amount;
    }

    public string ProfessionID;
    public EquipmentLoadout[] EquippedItems;
    public InventoryLoadout[] InventoryItems;
    public AttributeOverride[] AttributeOverrides;
}
