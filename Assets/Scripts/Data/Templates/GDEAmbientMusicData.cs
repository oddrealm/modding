using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Sound/AmbientMusic", order = 36)]
public class GDEAmbientMusicData : Scriptable
{
    public string IntroID = "";
    public string LoopID = "";
    public int MinHour = 0;
    public int MaxHour = 24;
}
