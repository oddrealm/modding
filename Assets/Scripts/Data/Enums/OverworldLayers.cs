
[System.Flags]
public enum OverworldLayers
{
    NONE = 0,
    TERRAIN = 1,
    RIVERS = 2,
    SETTLEMENTS = 4,
    BORDERS = 8,
    PATHS = 16,
    ROADS = 32,
    ALL = TERRAIN | RIVERS | SETTLEMENTS | BORDERS | PATHS | ROADS
}
