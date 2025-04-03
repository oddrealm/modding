using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Biome/BiomeWeather", order = 12)]
public class GDEBiomeWeatherData : Scriptable
{
    [System.Serializable]
    public struct SkylightLocationSpawn
    {
        public string SpawnID;
        public bool SpawnRandomFlora;
        public int MinTemp;
        public int MaxTemp;
        public uint ChanceToSpawn;
        public uint ChanceToSpawnSize;
    }

    public string UIDisplayAnimKey = "";
    public float FlammabilityMod = 0.0f;
    public int WeatherType = 0;
    public int NightOffset = 0;
    public int DayOffset = 0;
    public int MinTemp = 0;
    public int MaxTemp = 0;
    public int FXIndex = 0;
    public int ChanceToStart = 0;
    public int MinHourDuration = 0;
    public int HourDurationVariance = 0;
    public float Brightness = 0.0f;
    public string AquaticAmbientSFX = "";
    public string SubterraneanAmbientSFX = "";
    public string DaytimeAmbientSFX = "";
    public string NighttimeAmbientSFX = "";

    public bool WatersPlants = false;

    [Header("Tag Obj to spawn at the last skylight location")]
    public SkylightLocationSpawn[] SkylightSpawns = new SkylightLocationSpawn[]{
        new SkylightLocationSpawn()
        {
            SpawnRandomFlora = true,
            MinTemp = -99,
            MaxTemp = 99,
            ChanceToSpawn = 1,
            ChanceToSpawnSize = 10000
        },
        new SkylightLocationSpawn()
        {
            SpawnID = "block_fill_snow",
            SpawnRandomFlora = false,
            MinTemp = -99,
            MaxTemp = 0,
            ChanceToSpawn = 1,
            ChanceToSpawnSize = 100
        }
    };
}
