
public readonly struct NewTagObjParams
{
    public readonly SimulationState simState;
    public readonly InstanceUID groupUID;
    public readonly InstanceUID sourceUID;
    public readonly bool disposeIfNotPermitted;
    public readonly bool disableSpawnAtEntityOnFail;

    public NewTagObjParams(
        SimulationState simState,
        InstanceUID groupUID,
        InstanceUID sourceUID,
        bool disposeIfNotPermitted,
        bool disableSpawnAtEntityOnFail)
    {
        this.simState = simState;
        this.groupUID = groupUID;
        this.sourceUID = sourceUID;
        this.disposeIfNotPermitted = disposeIfNotPermitted;
        this.disableSpawnAtEntityOnFail = disableSpawnAtEntityOnFail;
    }

    public NewTagObjParams(
        SimulationState simState,
        NewTagObjParams newTagObjParams)
    {
        this.simState = simState;
        groupUID = newTagObjParams.groupUID;
        sourceUID = newTagObjParams.sourceUID;
        disposeIfNotPermitted = newTagObjParams.disposeIfNotPermitted;
        disableSpawnAtEntityOnFail = newTagObjParams.disableSpawnAtEntityOnFail;
    }

    public NewTagObjParams(SimulationState simState)
    {
        this.simState = simState;
        groupUID = 0;
        sourceUID = 0;
        disposeIfNotPermitted = false;
        disableSpawnAtEntityOnFail = false;
    }

#if ODD_REALM_APP
    public static readonly NewTagObjParams Default = new(
        simState: SimulationState.NewAgeWithTimeToSim(0),
        groupUID: 0,
        sourceUID: 0,
        disposeIfNotPermitted: false,
        disableSpawnAtEntityOnFail: false
    );

    public static readonly NewTagObjParams DefaultNoClaimCheck = new(
        simState: SimulationState.NewAgeWithTimeToSim(0),
        groupUID: 0,
        sourceUID: 0,
        disposeIfNotPermitted: false,
        disableSpawnAtEntityOnFail: false
    );
#endif
}
