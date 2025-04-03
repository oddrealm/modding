using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Simulation", order = 0)]
public class GDESimulationData : Scriptable
{
    [SerializeReference]
    public InstanceSimulation[] Simulations = new InstanceSimulation[] { };
}
