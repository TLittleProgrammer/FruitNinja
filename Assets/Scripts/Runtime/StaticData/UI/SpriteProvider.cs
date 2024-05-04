using System;
using System.Collections.Generic;
using Runtime.Infrastructure.SlicableObjects;
using UnityEngine;

namespace Runtime.StaticData.UI
{
    [CreateAssetMenu(menuName = "Providers/Sprite Provider", fileName = "SpriteProvider")]
    public sealed class SpriteProvider : ScriptableObject
    {
        public List<SpriteData> SlicableDictionary;
        public List<SpriteDataSecond> IconsByType;
    }

    [Serializable]
    public struct SpriteData
    {
        public string Id;
        public Sprite Sprite;
    }
    
    [Serializable]
    public struct SpriteDataSecond
    {
        public SlicableObjectType Id;
        public Sprite Sprite;
    }
}