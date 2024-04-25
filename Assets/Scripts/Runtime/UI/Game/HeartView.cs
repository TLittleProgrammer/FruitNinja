using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Runtime.UI.Game
{
    [RequireComponent(typeof(RectTransform))]
    public class HeartView : MonoBehaviour
    {
        [SerializeField] private Transform _heartTransform;
        [SerializeField] private float _duration;
        
        private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform;
        public Transform HeartTransform => _heartTransform;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public async UniTask AnimateGetDamage()
        {
            await Animate(0f);
        }

        public async UniTask AnimateGetHealth()
        {
            await Animate(1f);
        }

        private async Task Animate(float endValue)
        {
            await _heartTransform.DOScale(endValue, _duration).ToUniTask();
        }
    }
}