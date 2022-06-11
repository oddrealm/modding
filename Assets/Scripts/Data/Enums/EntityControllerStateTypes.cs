
[System.Flags]
public enum EntityControllerStateTypes
{
    NONE = 0,
    IDLE = 1,
    SLEEPING = 2,
    WORKING = 4,
    DEAD = 8,
    COMBAT = 16,
    FORCE_STOP = 32,
    MOVE_ORDER = 64,
    MATING = 128,
    HOLD_POSITION = 256,
    FREEZING = 512,
    THIRSTY = 1024,
    MANAGE_INVENTORY = 2048,
    VOID_SICK = 4096,
    HUNGRY = 8192,
    DROWNING = 16384,
    LEAVING = 32768,
    WORK_TARGET = 65536,
    ALL = IDLE |
        SLEEPING |
        WORKING |
        DEAD |
        COMBAT |
        FORCE_STOP |
        MOVE_ORDER |
        MATING |
        HOLD_POSITION |
        FREEZING |
        THIRSTY |
        MANAGE_INVENTORY |
        VOID_SICK |
        HUNGRY |
        DROWNING |
        LEAVING |
        WORK_TARGET
}