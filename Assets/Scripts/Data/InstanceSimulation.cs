using UnityEngine;

public interface ISimulationActionSource
{
    SimulationAction[] GetActions();
    void SetActions(SimulationAction[] actions);
}

public interface ISimulationConditionSource
{
    SimulationCondition[] GetConditions();
    void SetConditions(SimulationCondition[] conditions);
}

[System.Serializable]
public class InstanceSimulation : ISimulationActionSource, ISimulationConditionSource
{
    public string Comment = "Instance Simulation";
    public bool IsEnabled = true;


    [Header("Where we want to act")]
    [SerializeReference]
    public SimulationTarget Target = new SimulationTarget();

    [Header("If true, all conditions must pass for the action to activate")]
    public bool RequireAllConditionsToBeMet = true;
    [Header("What are the requirements to act")]
    [SerializeReference]
    public SimulationCondition[] Conditions = new SimulationCondition[] { };
    [Header("What happens during the act")]
    [SerializeReference]
    public SimulationAction[] Actions = new SimulationAction[] { };

    [System.NonSerialized]
    public bool DebugBreakpoint;
    [System.NonSerialized]
    public bool DebugMaximized;

    public SimulationAction[] GetActions() { return Actions; }
    public SimulationCondition[] GetConditions() { return Conditions; }
    public void SetActions(SimulationAction[] actions) { Actions = actions; }
    public void SetConditions(SimulationCondition[] conditions) { Conditions = conditions; }

    public void Init()
    {
        for (int i = 0; Conditions != null && i < Conditions.Length; i++)
        {
            Conditions[i].Init();
        }
    }

    public override string ToString()
    {
        return Comment;
    }
}
