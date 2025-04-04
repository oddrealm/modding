using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GlobalStat")]
public class GDEGlobalStatData : Scriptable
{
    public float DefaultValue = 0;
    public AttributeDisplayTypes DisplayType;
    public const string XP_MOD = "global_stat_xp_mod";
}
