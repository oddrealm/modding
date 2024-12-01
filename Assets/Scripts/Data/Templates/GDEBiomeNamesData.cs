using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BiomeNames")]
public class GDEBiomeNamesData : Scriptable
{
    public List<string> Determiner = new List<string>();
    public List<string> Names = new List<string>();
}
