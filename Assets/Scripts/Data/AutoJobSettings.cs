using System.Collections;
using UnityEngine;

namespace Assets.GameData
{
    [System.Serializable]
    public struct AutoJobSettings
    {
        public int Priority;
        public bool Enabled;
        public AutoJobTypes AutoJobType;
    }
}