[System.Flags]
public enum InventoryTypes
{
    NONE = 0,
    ONE_HAND = 1,
    TWO_HAND = 2,
    HEAD = 4,
    BODY = 8,
    LEGS = 16,
    FEET = 32,
    BACK = 64,
    ALL = ONE_HAND | TWO_HAND | HEAD | BODY | LEGS | FEET | BACK,
}
