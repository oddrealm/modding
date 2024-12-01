using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Names")]
public class GDEEntityNamesData : Scriptable
{
    public int FirstNameMasculinePrependChances = 0;
    public List<string> FirstNameMasculinePrepends = new List<string>();
    public List<string> MasculineFirstNames = new List<string>();
    public int FirstNameFemininePrependChances = 0;
    public List<string> FirstNameFemininePrepends = new List<string>();
    public List<string> FeminineFirstNames = new List<string>();
    public int LastNamePrependChances = 0;
    public List<string> LastNamePrepends = new List<string>();
    public List<string> LastNames = new List<string>();
}
