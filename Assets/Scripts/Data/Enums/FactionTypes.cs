[System.Flags]
public enum FactionTypes : byte
{
    NONE = 0,
    PLAYER = 1,
    NEUTRAL = 2,
    HOSTILE = 4,
    CAPTURED = 8,
    ALL = PLAYER | NEUTRAL | HOSTILE | CAPTURED
}
