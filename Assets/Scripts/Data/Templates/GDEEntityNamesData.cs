using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Names")]
public class GDEEntityNamesData : Scriptable
{
    public bool SeparateFirstNamePrepends = false;
    public bool SeparateFirstNameAppends = false;
    [Header("Neutral Names")]
    public int FirstNameNeutralPrependChances = 0;
    public List<string> FirstNameNeutralPrepends = new();
    public List<string> NeutralFirstNames = new();
    public int FirstNameNeutralAppendChances = 0;
    public List<string> FirstNameNeutralAppends = new();
    [Header("Masculine Names")]
    public int FirstNameMasculinePrependChances = 0;
    public List<string> FirstNameMasculinePrepends = new();
    public List<string> MasculineFirstNames = new();
    public int FirstNameMasculineAppendChances = 0;
    public List<string> FirstNameMasculineAppends = new();
    [Header("Feminine Names")]
    public int FirstNameFemininePrependChances = 0;
    public List<string> FirstNameFemininePrepends = new();
    public List<string> FeminineFirstNames = new();
    public int FirstNameFeminineAppendChances = 0;
    public List<string> FirstNameFeminineAppends = new();
    [Header("Last Names")]
    public bool SeparateLastNamePrepends = false;
    public int LastNamePrependChances = 0;
    public List<string> LastNamePrepends = new();
    public List<string> LastNames = new();
}
