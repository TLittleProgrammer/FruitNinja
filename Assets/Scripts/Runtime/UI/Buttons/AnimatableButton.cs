using DG.Tweening;
using Runtime.StaticData.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class AnimatableButton : MonoBehaviour
    {
        [SerializeField] private ButtonAnimationSettings _buttonAnimationSettings;

        private Button _button;
        
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
        }
        
        private void OnButtonClicked()
        {
            transform
                .DOScale(_buttonAnimationSettings.TargetScale, _buttonAnimationSettings.Duration)
                .SetEase(_buttonAnimationSettings.Ease)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}