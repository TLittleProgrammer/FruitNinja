using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Infrastructure.Effects;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Combo
{
    [RequireComponent(typeof(RectTransform))]
    public class ComboView : PopupEffect
    {
        [SerializeField] private TMP_Text _fruitsCounter;
        [SerializeField] private TMP_Text _xCounterText;

        private RectTransform _rectTransform;
        private string _fruitsCounterInitialText;
        private string _xCounterInitialText;
        private Vector2 _rectSize;

        public Vector2 RectSize => _rectSize;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _fruitsCounterInitialText = _fruitsCounter.text;
            _xCounterInitialText = _xCounterText.text;

            _rectSize = new Vector2(_rectTransform.rect.width / 2f, _rectTransform.rect.height / 2f);
        }


        public void ShowCombo(int fruits)
        {
            PlayAnimation(fruits);
        }

        public void SetPosition(Vector2 position)
        {
            _rectTransform.position = position;
        }

        private void PlayAnimation(int fruits)
        {
            _fruitsCounter.text = String.Format(_fruitsCounter.text, fruits);
            _xCounterText.text = String.Format(_xCounterText.text, fruits);
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;

            Sequence = DOTween.Sequence();

            Sequence.Append(transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InQuad)).ToUniTask().Forget();
            Sequence.AppendInterval(1f).ToUniTask().Forget();
            Sequence.Append(transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuint)).ToUniTask().Forget();
            Sequence.OnComplete(() =>
            {
                CanPause = true;
                _fruitsCounter.text = _fruitsCounterInitialText;
                _xCounterText.text  = _xCounterInitialText;
                
                gameObject.SetActive(false);
            }).ToUniTask().Forget();
        }

        public class Pool : MonoMemoryPool<ComboView>
        {
        }
    }
}