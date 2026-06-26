using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AudioGroups")]
public class GDEAudioGroupsData : Scriptable
{
    public float PitchOverride = 1f;
    public RandomChance ChanceToPlay;
    public List<string> SFX = new List<string>();
}
