using System.Collections.Generic;
using Runtime.Infrastructure.SlicableObjects;
using UnityEngine;

namespace Runtime.StaticData.Boosts
{
    [CreateAssetMenu(menuName = "Settings/Game/Mimik", fileName = "MimikStaticData")]
    public sealed class MimikSettings : ScriptableObject
    {
        public float Offset;
        public float SpecialIntervalBeforeShit;
        public float Scale;
        public List<SlicableObjectType> AvailableTypes;
    }
}