using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Simulation")]
public class GDESimulationData : Scriptable
{
    [SerializeReference]
    public InstanceSimulation[] Simulations = new InstanceSimulation[] { };
}
