using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterColorMaskData")]
public class GDECharacterColorMaskData : Scriptable
{
    [System.Serializable]
    public class BodyPartColorPair
    {
#if UNITY_EDITOR
        [EnumFlag]
#endif
        public EntityBodyParts BodyPart = EntityBodyParts.ALL;
        public Color Target = Color.magenta;
    }

    public int Priority = 0;

    public BodyPartColorPair[] Pairs;
}
