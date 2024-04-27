using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.StaticData.UI
{
    [CreateAssetMenu(menuName = "Providers/Sprite Provider", fileName = "SpriteProvider")]
    public sealed class SpriteProvider : ScriptableObject
    {
        public List<SpriteData> SlicableDictionary;
    }

    [Serializable]
    public struct SpriteData
    {
        public string Id;
        public Sprite Sprite;
    }
}