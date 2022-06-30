
[System.Flags]
public enum StoragePreferences
{
    NONE = 0,
    CONTAINER = 1,
    OPEN_BLOCK = 2,
    ALL = CONTAINER | OPEN_BLOCK,
}
