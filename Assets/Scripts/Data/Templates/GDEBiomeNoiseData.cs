using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BiomeTerrainNoiseTuning")]
public class GDEBiomeNoiseData : Scriptable
{
    [Header("The closeness or distance of the terrain's hills and valleys")]
    public float Frequency = 0.0f;

    [Header("The number of layers in the noise, each contributing to the overall shape, with higher octaves adding finer detail to the landscape.")]
    public int Octaves = 0;

    // if Lacunarity is 2 (a common value), the frequency of each successive layer of noise will be twice that of the previous one.
    // A higher Lacunarity value will cause the frequency to increase more quickly, leading to more gaps or holes in the terrain.
    // This can create a complex and chaotic landscape with a more rugged appearance.
    [Header("Increases frequency each octave")]
    public float Lacunarity = 0.0f;

    [Header("The rate at which amplitude changes across octaves, influencing smoothness or detail")]
    public float Persistence = 0.0f;

    [Header("Middle Pleateau"), Range(0f, 1f)]
    public float MiddlePlateauStrength;

    [Header("Peaks"), Range(0f, 1f)]
    public float PeakStrength;

    [Header("Valleys"), Range(0f, 1f)]
    public float ValleyStrength;
    [Range(0f, 1f)]
    public float ValleyTarget;

    [Header("Sample Falloff"), Range(0f, 1f)]
    public float FalloffStrength;

    [Header("Hermite"), Range(0f, 1f)]
    public float HermiteStrength;

    [Header("Plateau"), Range(0f, 1f)]
    public float PlateauStrength;
    [Range(0f, 1f)]
    public float PlateauTarget;
}