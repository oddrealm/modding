using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Actions")]
public class GDEActionData : Scriptable
{
    public RandomChance ActivationChance = new RandomChance(1, 100);
    public BuffData[] Buffs = System.Array.Empty<BuffData>();
    public string[] Statuses = System.Array.Empty<string>();
    public string[] Spawns = System.Array.Empty<string>();
    public int Duplicate = 0;
}
