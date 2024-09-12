using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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