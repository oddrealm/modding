
[System.Serializable]
public struct NewTagObjParams
{
    public SimulationState SimState;
    public InstanceGroupUID GroupUID;
    public InstanceUID Source;
    public bool DisableRemoveSpawns;
    public bool DisposeIfNotPermitted;
    public bool DisableSpawnAtEntityOnFail;
    public bool DisableClaimCheck;
    public bool SetAsUnclaimed;
    public int Attempts;
    public int SeedOffset;

    public static readonly NewTagObjParams Default = new NewTagObjParams
    {
        SimState = 0,
        GroupUID = 0,
        Source = 0,
        DisableRemoveSpawns = false,
        DisposeIfNotPermitted = false,
        DisableSpawnAtEntityOnFail = false,
        DisableClaimCheck = false,
        SetAsUnclaimed = false,
        Attempts = 0,
        SeedOffset = 0,
    };

    public static readonly NewTagObjParams DefaultNoRemoveSpawns = new NewTagObjParams
    {
        SimState = 0,
        GroupUID = 0,
        Source = 0,
        DisableRemoveSpawns = true,
        DisposeIfNotPermitted = false,
        DisableSpawnAtEntityOnFail = false,
        DisableClaimCheck = false,
        SetAsUnclaimed = false,
        Attempts = 0,
        SeedOffset = 0,
    };

    public static readonly NewTagObjParams DefaultNoClaimCheck = new NewTagObjParams
    {
        SimState = 0,
        GroupUID = 0,
        Source = 0,
        DisableRemoveSpawns = false,
        DisposeIfNotPermitted = false,
        DisableSpawnAtEntityOnFail = false,
        DisableClaimCheck = true,
        SetAsUnclaimed = false,
        Attempts = 0,
        SeedOffset = 0,
    };
}
