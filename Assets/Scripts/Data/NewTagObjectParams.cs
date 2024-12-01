
[System.Serializable]
public struct NewTagObjParams
{
    public SimulationState simState;
    public InstanceGroupUID groupUID;
    public InstanceUID sourceUID;
    public bool disableRemoveSpawns;
    public bool disposeIfNotPermitted;
    public bool disableSpawnAtEntityOnFail;
    public bool disableClaimCheck;
    public bool setAsUnclaimed;
    public int attempts;
    public int seedOffset;

    public static readonly NewTagObjParams Default = new NewTagObjParams
    {
        simState = 0,
        groupUID = 0,
        sourceUID = 0,
        disableRemoveSpawns = false,
        disposeIfNotPermitted = false,
        disableSpawnAtEntityOnFail = false,
        disableClaimCheck = false,
        setAsUnclaimed = false,
        attempts = 0,
        seedOffset = 0,
    };

    public static readonly NewTagObjParams DefaultNoRemoveSpawns = new NewTagObjParams
    {
        simState = 0,
        groupUID = 0,
        sourceUID = 0,
        disableRemoveSpawns = true,
        disposeIfNotPermitted = false,
        disableSpawnAtEntityOnFail = false,
        disableClaimCheck = false,
        setAsUnclaimed = false,
        attempts = 0,
        seedOffset = 0,
    };

    public static readonly NewTagObjParams DefaultNoClaimCheck = new NewTagObjParams
    {
        simState = 0,
        groupUID = 0,
        sourceUID = 0,
        disableRemoveSpawns = false,
        disposeIfNotPermitted = false,
        disableSpawnAtEntityOnFail = false,
        disableClaimCheck = true,
        setAsUnclaimed = false,
        attempts = 0,
        seedOffset = 0,
    };
}
