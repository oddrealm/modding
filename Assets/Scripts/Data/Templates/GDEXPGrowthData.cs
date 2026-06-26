using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/XPGrowth")]
public class GDEXPGrowthData : Scriptable
{
    public AnimationCurve Curve;
    public int MaxLevel = 99;
    public int BaseXP = 250;
    public int MaxXP = 100000;

    private readonly System.Collections.Generic.Dictionary<int, int> xpTable = new();

    public int GetXPForLevel(int level)
    {
        if (xpTable.TryGetValue(level, out int xp))
        {
            return xp;
        }

        if (level < 0 || level > MaxLevel)
        {
            Debug.LogError($"Level must be between 0 and {MaxLevel}");
            return int.MaxValue;
        }

        xp = SampleCurve(level);
        xpTable[level] = xp;
        return xp;
    }

    private int SampleCurve(int lvl)
    {
        if (lvl < 0 || lvl > MaxLevel)
        {
            Debug.LogError($"Level must be between 0 and {MaxLevel}");
            return int.MaxValue;
        }

        float normalizedLevel = (float)lvl / MaxLevel;
        float curveValue = Curve.Evaluate(normalizedLevel);
        int xp = Mathf.RoundToInt(BaseXP + curveValue * (MaxXP - BaseXP));
        return xp;
    }

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        for (int level = 0; level <= MaxLevel; level++)
        {
            xpTable[level] = SampleCurve(level);
        }

        base.OnLoaded();
    }
#endif
}
