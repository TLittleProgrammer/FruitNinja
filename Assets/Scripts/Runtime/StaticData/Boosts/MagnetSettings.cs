﻿using System.Collections.Generic;
using Runtime.Infrastructure.SlicableObjects;
using UnityEngine;

namespace Runtime.StaticData.Boosts
{
    [CreateAssetMenu(menuName = "Settings/Game/Magnet", fileName = "MagnetStaticData")]
    public sealed class MagnetSettings : ScriptableObject
    {
        public float Force;
        public float Duration;
        public float Distance;
        public List<SlicableObjectType> AvailableMagnetTypes;
    }
}