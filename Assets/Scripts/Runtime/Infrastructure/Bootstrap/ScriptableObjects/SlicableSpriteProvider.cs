﻿using System;
using System.Collections.Generic;
using Runtime.SlicableObjects;
using UnityEngine;

namespace Runtime.Infrastructure.Bootstrap.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/Settings/Slicable Sprite Provider", fileName = "SlicableSpriteProvider")]
    public class SlicableSpriteProvider : ScriptableObject
    {
        public List<SlicableDictionary> SlicableDictionary;
    }

    [Serializable]
    public struct SlicableDictionary
    {
        public SlicableObjectType SlicableObjectType;
        public SlicableItem SlicableItem;
    }

    [Serializable]
    public struct SlicableItem
    {
        public List<Sprite> Sprites;
    }
}