[System.Flags]
public enum EntityScenes
{
    NONE = 0,
    LIMBO = 1,
    IN_TILE = 2,
    IN_PARTY = 4,

    ACTIVE = IN_TILE | IN_PARTY
}