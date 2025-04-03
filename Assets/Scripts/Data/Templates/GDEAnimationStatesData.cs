using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Animation/AnimationStates", order = 3)]
public class GDEAnimationStatesData : Scriptable
{
    [System.Serializable]
    public class Transition
    {
        public GDEAnimationStatesData NextState;
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
