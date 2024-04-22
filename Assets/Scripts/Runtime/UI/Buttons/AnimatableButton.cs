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

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
            _sequence.Kill();
        }
        
        private void OnButtonClicked()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(transform
                .DOScale(_buttonAnimationSettings.TargetScale, _buttonAnimationSettings.Duration)
                .SetEase(_buttonAnimationSettings.Ease)
                .SetLoops(2, LoopType.Yoyo));

            _sequence.Play();
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