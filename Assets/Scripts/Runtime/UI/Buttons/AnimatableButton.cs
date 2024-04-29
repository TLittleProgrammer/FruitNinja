using DG.Tweening;
using Runtime.StaticData.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UI.Buttons
{
    public class AnimatableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private ButtonAnimationSettings _buttonAnimationSettings;
        
        private Sequence _sequence;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_sequence is not null && _sequence.active)
            {
                return;
            }

            _sequence = DOTween.Sequence();
            _sequence
                .Append(transform
                    .DOScale(_buttonAnimationSettings.TargetScale, _buttonAnimationSettings.Duration)
                    .SetEase(_buttonAnimationSettings.Ease))
                .OnComplete(() =>
                {
                    _sequence.Kill();
                });

            _sequence.Play();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_sequence is not null && _sequence.active)
            {
                return;
            }
            
            _sequence = DOTween.Sequence();
            _sequence
                .Append(transform
                    .DOScale(1f, _buttonAnimationSettings.Duration)
                    .SetEase(_buttonAnimationSettings.Ease))
                .OnComplete(() =>
                {
                    _sequence.Kill();
                });

            _sequence.Play();
        }
    }
}