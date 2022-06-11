using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterAccessoryData")]
public class GDECharacterAccessoryData : ScriptableObject
{
    public int Priority = 0;
    public int TexX = 69;
    public int TexY = 128;
    public Color ColorA = new Color(1f,1f,1f,0f);
    public Color ColorB = new Color(1f, 1f, 1f, 0f);
    public Color ColorC = new Color(1f, 1f, 1f, 0f);
    public Color ColorMaskA = Color.red;
    public Color ColorMaskB = Color.green;
    public Color ColorMaskC = Color.blue;
    public EntityAccessoryPoints DefaultAccessoryPoint = EntityAccessoryPoints.BACK;

    public GDEAnimationStateData[] HiddenAnimationStates;
}