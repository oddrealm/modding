using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Animation/AnimationSets", order = 1)]
public class GDEAnimationSetsData : Scriptable
{
    public GDEAnimationStatesData State;
    public GDEAnimationsData UpAnimation;
    public GDEAnimationsData DownAnimation;
    public GDEAnimationsData RightAnimation;
    public GDEAnimationsData LeftAnimation;
}
