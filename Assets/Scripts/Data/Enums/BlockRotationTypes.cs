[System.Flags]
public enum BlockRotationTypes
{
    NONE = 0,               // Does not rotate
    LOCKED_FIXTURE = 1,     // Player cannot rotate but will snap to fixtures
    FREE = 2,               // Player can rotate any direction
    FREE_FIXTURE = 4,       // Player can rotate to snap to any fixtures (walls)
    PLAYER_CAN_ROTATE = FREE | FREE_FIXTURE,
    CAN_ROTATE = LOCKED_FIXTURE | FREE | FREE_FIXTURE,
    FIXTURE = LOCKED_FIXTURE | FREE_FIXTURE
}