[System.Flags]
public enum NotificationFilterTypes
{
    NONE = 0,
    COMBAT = 1,
    BLUEPRINTS = 2,
    ENTITY = 4,
    NATURE = 8,
    WORK = 16,
    FISHING = 32,
    EVENT = 64, // Scenarios
    ALL = COMBAT | BLUEPRINTS | ENTITY | NATURE | WORK | FISHING | EVENT
}
