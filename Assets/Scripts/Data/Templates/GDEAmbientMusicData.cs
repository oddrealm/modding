using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AmbientMusic")]
public class GDEAmbientMusicData : Scriptable
{
    public string IntroID = "";
    public string LoopID = "";
    public int MinHour = 0;
    public int MaxHour = 24;
}
