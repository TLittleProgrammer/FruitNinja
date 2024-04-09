using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.UI.Screens
{
    [CreateAssetMenu(menuName = "Providers/Screens", fileName = "ScreenProvider")]
    public class ScreenProvider : ScriptableObject
    {
        public List<ScreenData> Screens;
    }

    [Serializable]
    public struct ScreenData
    {
        public ScreenType ScreenType;
        public GameObject GameObject;
    }
}