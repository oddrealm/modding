using UnityEngine;

[System.Serializable]
public struct RandomChance
{
    public uint Chance;
    public uint SampleSize;

    public RandomChance(uint chance, uint sampleSize)
    {
        Chance = (uint)Mathf.Min(chance, sampleSize);
        SampleSize = sampleSize;
    }

    public readonly float Percentage
    {
        get
        {
            if (Chance == 0 && SampleSize == 0)
            {
                return 1f;
            }

            return (float)Chance / (float)Mathf.Max(1, SampleSize);
        }
    }

    public readonly bool IsValid => Chance > 0 && SampleSize > 0;
    public readonly bool IsGuaranteed => Chance >= SampleSize;

#if ODD_REALM_APP
    public readonly bool Roll()
    {
        if (IsGuaranteed) { return true; }
        return TinyBeast.Random.Range(0, (int)SampleSize) < Chance;
    }
#endif
}
