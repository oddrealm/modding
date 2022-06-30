using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ItemCategories
{
    NONE = 0,
    RESOURCES = 1,
    FOOD_RAW = 2,
    FOOD_COOKED = 4,
    SEEDS = 8,
    GARBAGE = 16,
    REFINED_MATERIALS = 32,
    TOOLS = 64,
    GEAR = 128,
    BOOKS = 256,
    LIQUIDS = 512,
    BEVERAGES = 1024,
    REMAINS = 2048,
    UNUSED0 = 4096,
    UNUSED1 = 8192,
    END = 16384,
    FOOD = FOOD_RAW | FOOD_COOKED,

    ALL = RESOURCES | FOOD_RAW | FOOD_COOKED | SEEDS | GARBAGE | REFINED_MATERIALS | TOOLS | GEAR | BOOKS | LIQUIDS | BEVERAGES | REMAINS | UNUSED0 | UNUSED1 // 16127 (no custom)
}