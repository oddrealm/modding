using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemRarity")]
public class GDEItemRarityData : Scriptable
{
    public int RarityScore;
    public bool DisposeIfCannotSpawn = true;
}
