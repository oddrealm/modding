[System.Flags]
public enum BlockPlantTypes
{
    NONE = 0,
    GROUND_COVER = 1,
    STANDING = 2,
    TREES = 4,
    ALL = GROUND_COVER | STANDING | TREES
}