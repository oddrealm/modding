using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Names")]
public class GDEEntityNamesData : Scriptable
{
    public bool SeparateFirstNamePrepends = false;
    public int FirstNameNeutralPrependChances = 0;
    public List<string> FirstNameNeutralPrepends = new List<string>();
    public List<string> NeutralFirstNames = new List<string>();
    public int FirstNameMasculinePrependChances = 0;
    public List<string> FirstNameMasculinePrepends = new List<string>();
    public List<string> MasculineFirstNames = new List<string>();
    public int FirstNameFemininePrependChances = 0;
    public List<string> FirstNameFemininePrepends = new List<string>();
    public List<string> FeminineFirstNames = new List<string>();
    public bool SeparateLastNamePrepends = false;
    public int LastNamePrependChances = 0;
    public List<string> LastNamePrepends = new List<string>();
    public List<string> LastNames = new List<string>();
}
