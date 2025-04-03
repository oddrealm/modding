using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AutoJobEntityPool", order = 0)]
public class GDEAutoJobEntityPoolData : Scriptable
{
    public AutoJobEntityPoolTypes PoolType = AutoJobEntityPoolTypes.NONE;
}