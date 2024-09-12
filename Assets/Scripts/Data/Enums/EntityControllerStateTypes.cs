
[System.Flags]
public enum EntityControllerStateTypes
{
    NONE = 0,
    IDLE = 1,
    SLEEP = 2,
    WORK = 4,
    DEAD = 8,
    COMBAT = 16,
    FORCE_STOP = 32,
    MOVE_ORDER = 64,
    MATE = 128,
    HOLD_POSITION = 256,
    LEAVING = 32768,
    FOLLOW_TARGET = 65536,
    STUCK = 131072,
    STEALTH = 262144,
    WARM_UP = 524288,
}