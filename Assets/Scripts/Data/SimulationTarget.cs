using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimPositions
{
    [Header("These are offset from the origin")]
    [SerializeReference]
    public Vector3Int[] Positions = new Vector3Int[1];
    public int MaxDistance = 1;
    public int MinDistance = 1;
    public bool IterateToEnd;
    public bool CheckAll;

    public void AddPosition(BlockPoint p)
    {
        if (HasPosition(p)) { return; }

        Vector3Int[] prev = Positions;
        Positions = new Vector3Int[prev.Length + 1];

        Positions[Positions.Length - 1] = p.ToVector3Int();

        for (int i = 0; i < prev.Length; i++)
        {
            Positions[i] = prev[i];
        }
    }


    public void RemovePosition(BlockPoint p)
    {
        if (!HasPosition(p)) { return; }

        int index = -1;

        for (int i = 0; i < Positions.Length; i++)
        {
            if (index != -1)
            {
                Positions[i - 1] = Positions[i];
                continue;
            }

            if (Positions[i] != p.ToVector3Int()) { continue; }

            index = i;
        }

        Vector3Int[] prev = Positions;
        Positions = new Vector3Int[prev.Length - 1];

        for (int i = 0; i < Positions.Length; i++)
        {
            Positions[i] = prev[i];
        }
    }

    public void TogglePosition(BlockPoint p)
    {
        if (HasPosition(p))
        {
            RemovePosition(p);
        }
        else
        {
            AddPosition(p);
        }
    }

    public bool HasPosition(BlockPoint p)
    {
        for (int i = 0; i < Positions.Length; i++)
        {
            if (Positions[i] != p.ToVector3Int()) { continue; }

            return true;
        }

        return false;
    }
}

[System.Serializable]
public class SimulationTarget
{
    [SerializeReference]
    public SimPositions TargetPositions = new SimPositions();
}
