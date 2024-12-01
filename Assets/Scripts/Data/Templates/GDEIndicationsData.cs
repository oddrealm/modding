using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Indications")]
public class GDEIndicationsData : Scriptable
{
    public int Priority = 0;
    public string IconID = "";
    public float Duration = 0.0f;
    public float Interval = 0.0f;
    public bool Loop = false;
    public string MoveCurve = "";
    public string FadeCurve = "";
    public string ColorCurve = "";
}
