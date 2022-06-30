using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AnimationSetGroup")]
public class GDEAnimationSetGroupData : ScriptableObject
{
    public string Key;
    public GDEAnimationSetData[] Sets;
}
