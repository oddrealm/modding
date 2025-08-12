[System.Flags]
public enum EntityBodyParts
{
    NONE = 0,
    HEAD = 1,
    NECK = 2,
    SHOULDERS = 4,
    TORSO = 8,
    ARMS = 16,
    HANDS = 32,
    GROIN = 64,
    LEGS = 128,
    FEET = 256,
    ACCESSORIES = 512,

    ALL = HEAD | NECK | SHOULDERS | TORSO | ARMS | HANDS | GROIN | LEGS | FEET | ACCESSORIES,
}
