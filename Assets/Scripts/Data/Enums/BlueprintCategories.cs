[System.Flags]
public enum BlueprintCategories
{
    NONE = 0,
    BLOCKS = 1,
    FURNITURE = 2,
    TOOLS = 4,
    PLANTS = 8,
    FLOORS = 16,
    MATERIALS = 32,
    GEAR = 64,
    TREES = 128,
    ROOFS = 256,
    DOORS = 512,
    LIGHTING = 1024,
    MEALS = 2048,
    CONTAINERS = 4096,
    SUMMONS = 8192,
    BEVERAGES = 16384,
    VERT_ACCESS = 32768,
    WEEDS = 65536,
    MAINTENANCE = 131072,
    ALL = BLOCKS | 
          FURNITURE | 
          TOOLS | 
          PLANTS | 
          FLOORS | 
          MATERIALS | 
          GEAR | 
          TREES | 
          ROOFS | 
          DOORS | 
          LIGHTING | 
          MEALS | 
          CONTAINERS | 
          SUMMONS | 
          BEVERAGES | 
          VERT_ACCESS | 
          WEEDS | 
          MAINTENANCE
}