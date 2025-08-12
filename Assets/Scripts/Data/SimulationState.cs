public readonly struct SimulationState
{
    // The timestamp for when the sim obj was created.
    public readonly SimTime SpawnTimestamp;

    // The timestamp for when the sim obj was last simulated.
    public readonly SimTime LastSimulationTimestamp;

#if ODD_REALM_APP
    // The game's current minute.
    public SimTime CurrentTimestamp { get { return _session.GameTime.TotalElapsedMinutes; } }

    // How much time to simulate for the next sim update.
    public SimTime RemainingSimTime { get { return CurrentTimestamp - LastSimulationTimestamp; } }

    // The amount of simmed time total. This is the age of the sim obj.
    public SimTime SimAge { get { return LastSimulationTimestamp - SpawnTimestamp; } }

    public bool IsNULL { get { return SpawnTimestamp == 0; } }
    public static readonly SimulationState NULL = new SimulationState(0, 0);

    // This will sim from the last timestamp to current time.
    public SimulationState RemainingState { get { return new SimulationState(LastSimulationTimestamp, LastSimulationTimestamp); } }

    // This will give us a sim for the game's current time. So, it won't simulate anything new as the time delta is 0.
    public SimulationState CurrentState { get { return new SimulationState(CurrentTimestamp, CurrentTimestamp); } }

    private static GameSession _session;

    public static implicit operator SimulationState(int age)
    {
        return NewAgeWithTimeToSim((uint)age);
    }

    public static void Init(GameSession session)
    {
        _session = session;
    }

    public SimulationState(SimTime spawnTimestamp, SimTime lastSimulationTimestamp)
    {
        SpawnTimestamp = spawnTimestamp;
        LastSimulationTimestamp = lastSimulationTimestamp;
    }

    public static SimulationState NewAgeWithTimeToSim(uint age)
    {
        SimTime spawnTime = (_session.GameTime.TotalElapsedMinutes - age);
        SimTime lastSimTime = spawnTime;
        return new SimulationState(spawnTime, lastSimTime);
    }

    public static SimulationState NewAgeWithTimeToSim(SimTime age, SimTime timeToSim)
    {
        // timeToSim cannot be greater than age - we simulate objects up to their current age.
        if (timeToSim > age)
        {
            timeToSim = age;
        }

        SimTime spawnTime = (_session.GameTime.TotalElapsedMinutes - age);
        SimTime lastSimTime = (_session.GameTime.TotalElapsedMinutes - timeToSim);
        return new SimulationState(spawnTime, lastSimTime);
    }

    public static void Save(ES2Writer writer, SimulationState simState)
    {
        writer.Write((uint)simState.SpawnTimestamp);
        writer.Write((uint)simState.LastSimulationTimestamp);
    }

    public static SimulationState Load(ES2Reader reader)
    {
        uint spawnTime = reader.Read<uint>();
        uint lastSimTime = reader.Read<uint>();
        return new SimulationState(spawnTime, lastSimTime);
    }

    public override string ToString()
    {
        return $"Age: {SimAge}, Remaining Sim Time: {RemainingSimTime}, Spawn Time: {SpawnTimestamp}, Last Sim Time: {LastSimulationTimestamp}";
    }
#endif
}
