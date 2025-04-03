using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Entity/EntityAge", order = 18)]
public class GDEEntityAgeData : Scriptable
{
    public EntityAgeTypes AgeType;
    public int Max = int.MaxValue;
    public int Min = 0;
    public bool CanRespec = true;
    public List<AgeStatuses> Statuses = new List<AgeStatuses>();

    [System.Serializable]
    public class AgeStatuses
    {
        public string StatusID;
        public int Chances = 10000;
        public bool AddAgeToChances = true;
    }
}
