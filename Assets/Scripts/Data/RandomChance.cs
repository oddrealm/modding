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

    public float Percentage
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

    public bool IsValid => Chance > 0 && SampleSize > 0;

#if ODD_REALM_APP
    public bool Roll()
    {
        if (Chance == SampleSize) { return true; }
        return TinyBeast.Random.Range(0, (int)SampleSize) < Chance;
    }
#endif
}
