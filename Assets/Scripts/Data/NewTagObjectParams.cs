
public readonly struct NewTagObjParams
{
    public readonly SimulationState simState;
    public readonly InstanceGroupUID groupUID;
    public readonly InstanceUID sourceUID;
    public readonly bool disposeIfNotPermitted;
    public readonly bool disableSpawnAtEntityOnFail;
    public readonly bool disableClaimCheck;
    public readonly bool setAsUnclaimed;
    public readonly int seedOffset;

    public NewTagObjParams(
        SimulationState simState,
        InstanceGroupUID groupUID,
        InstanceUID sourceUID,
        bool disposeIfNotPermitted,
        bool disableSpawnAtEntityOnFail,
        bool disableClaimCheck,
        bool setAsUnclaimed,
        int seedOffset)
    {
        this.simState = simState;
        this.groupUID = groupUID;
        this.sourceUID = sourceUID;
        this.disposeIfNotPermitted = disposeIfNotPermitted;
        this.disableSpawnAtEntityOnFail = disableSpawnAtEntityOnFail;
        this.disableClaimCheck = disableClaimCheck;
        this.setAsUnclaimed = setAsUnclaimed;
        this.seedOffset = seedOffset;
    }

    public NewTagObjParams(
        SimulationState simState,
        NewTagObjParams newTagObjParams)
    {
        this.simState = simState;
        this.groupUID = newTagObjParams.groupUID;
        this.sourceUID = newTagObjParams.sourceUID;
        this.disposeIfNotPermitted = newTagObjParams.disposeIfNotPermitted;
        this.disableSpawnAtEntityOnFail = newTagObjParams.disableSpawnAtEntityOnFail;
        this.disableClaimCheck = newTagObjParams.disableClaimCheck;
        this.setAsUnclaimed = newTagObjParams.setAsUnclaimed;
        this.seedOffset = newTagObjParams.seedOffset;
    }

    public NewTagObjParams(SimulationState simState)
    {
        this.simState = simState;
        this.groupUID = 0;
        this.sourceUID = 0;
        this.disposeIfNotPermitted = false;
        this.disableSpawnAtEntityOnFail = false;
        this.disableClaimCheck = false;
        this.setAsUnclaimed = false;
        this.seedOffset = 0;
    }

#if ODD_REALM_APP
    public static readonly NewTagObjParams Default = new NewTagObjParams(
        simState: 0,
        groupUID: 0,
        sourceUID: 0,
        disposeIfNotPermitted: false,
        disableSpawnAtEntityOnFail: false,
        disableClaimCheck: false,
        setAsUnclaimed: false,
        seedOffset: 0
    );

    public static readonly NewTagObjParams DefaultNoClaimCheck = new NewTagObjParams(
        simState: 0,
        groupUID: 0,
        sourceUID: 0,
        disposeIfNotPermitted: false,
        disableSpawnAtEntityOnFail: false,
        disableClaimCheck: true,
        setAsUnclaimed: false,
        seedOffset: 0
    );
#endif
}
