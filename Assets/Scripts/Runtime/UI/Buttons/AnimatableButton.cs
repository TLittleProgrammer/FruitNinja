using DG.Tweening;
using Runtime.StaticData.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class AnimatableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private ButtonAnimationSettings _buttonAnimationSettings;

        private Button _button;
        private Sequence _sequence;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnDisable()
        {
            _sequence.Kill();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(transform
                .DOScale(_buttonAnimationSettings.TargetScale, _buttonAnimationSettings.Duration)
                .SetEase(_buttonAnimationSettings.Ease));

            _sequence.Play();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(transform
                .DOScale(1f, _buttonAnimationSettings.Duration)
                .SetEase(_buttonAnimationSettings.Ease));

            _sequence.Play();
        }
    }
}