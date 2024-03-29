﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AnimationState")]
public class GDEAnimationStateData : ScriptableObject
{
    public string Key;

    [System.Serializable]
    public class Transition
    {
        public GDEAnimationStateData NextState;
        public EntityAnimationFlags TrueFlags;
        public EntityAnimationFlags FalseFlags;
        public EntityAnimationTriggers Triggers;
        public bool IsComplete;

        public Transition() { }

        public void Paste(Transition other)
        {
            NextState = other.NextState;
            TrueFlags = other.TrueFlags;
            FalseFlags = other.FalseFlags;
            Triggers = other.Triggers;
            IsComplete = other.IsComplete;
        }
    }

    public Transition[] Transitions;
}
