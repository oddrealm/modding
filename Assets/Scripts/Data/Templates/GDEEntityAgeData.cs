﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntityAge")]
public class GDEEntityAgeData : Scriptable
{
    public EntityAgeTypes AgeType;
    public int Max = int.MaxValue;
    public int Min = 0;
    public bool CanEarnXP = true;
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
