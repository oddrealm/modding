using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AnimationSet")]
public class GDEAnimationSetData : ScriptableObject
{
    public GDEAnimationStateData State;
    public GDEAnimationsData UpAnimation;
    public GDEAnimationsData DownAnimation;
    public GDEAnimationsData RightAnimation;
    public GDEAnimationsData LeftAnimation;
}
