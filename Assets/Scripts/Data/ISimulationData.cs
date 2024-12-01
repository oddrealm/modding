public interface ISimulationData : ITooltipContent
{
    void SetSimulationID(string simID);
    string SimulationID { get; }
    GDESimulationData Simulation { get; }
    SimTime MaxSimTime { get; }

    string Key { get; }
    string[] GetSimStates();
    SimOptions[] GetOptions();
}