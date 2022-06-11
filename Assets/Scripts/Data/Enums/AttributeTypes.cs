
public enum AttributeTypes : uint
{
    NONE = 0, 
    HEALTH = 1,
    TOUGHNESS = 2,
    EVASION = 3,
    ENERGY = 4,
    XP = 5,
    LEVEL = 6,
    AMBIENT_TEMPERATURE = 7,
    AGE = 8,
    HUNGER = 9,

    ENERGY_CHANGE = 10,
    FISH_LURE = 11,
    FISH_SCARE = 12,
    CRIT_RATE = 13,
    HEALTH_CHANGE = 14,
    DAMAGE_IGNORE_RATE = 15,
    ATTACK_SPEED_MOD = 16,
    PROJECTILE_MULT_RATE = 17,
    PROJECTILE_MULT_MOD = 18,
    OUTPUT_QUALITY_MAX = 19,
    RESOURCE_USAGE_DECREASE_RATE = 20,
    RESOURCE_USAGE_DECREASE_MOD = 21,
    RESOURCE_FIND_RATE = 22,
    RESOURCE_FIND_MOD = 23,
    HUNGER_CHANGE = 24,

    HAPPINESS = 25,

    // v52
    THIRST = 26,
    THIRST_CHANGE = 27,

    // v53
    HAPPINESS_CHANGE = 28,

    // v78
    TEMPERATURE = 29,
    COLD_TOLERANCE = 30,

    // v89
    MOVE_SPEED = 31,

    // v91
    CLIMB_SPEED = 32,

    BURN_RATE = 33,
    BURN_MOD = 34,

    OXYGEN = 35,
    OXYGEN_CHANGE = 36,

    SIGHT_RANGE = 37
}

public enum AttributeDisplayTypes
{
    NONE = 0,
    AMOUNT = 1,
    MAX = 2,
    AMOUNT_AND_MAX = 3,
    PERCENT = 4,
    PERCENT_INT = 5

}

public enum AttributeColorTypes
{
    NONE = 0,
    HEX = 1,
    AMOUNT_TO_TEMP = 2,
    GREEN_TO_RED = 3,
    RED_TO_GREEN = 4
}