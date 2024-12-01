using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AnimationSets")]
public class GDEAnimationSetsData : Scriptable
{
    public GDEAnimationStatesData State;
    public GDEAnimationsData UpAnimation;
    public GDEAnimationsData DownAnimation;
    public GDEAnimationsData RightAnimation;
    public GDEAnimationsData LeftAnimation;
}
