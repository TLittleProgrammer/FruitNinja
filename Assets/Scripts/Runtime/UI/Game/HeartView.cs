using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Game
{
    public class HeartView : MonoBehaviour
    {
        [SerializeField] private Image _leftHeart;
        [SerializeField] private Image _rightHeart;
        [SerializeField] private float _duration;

        private bool _isFirstAnimate = true;
        
        public void AnimateGetDamage()
        {
            if (_isFirstAnimate)
            {
                _leftHeart.DOFillAmount(0f, _duration);
                _isFirstAnimate = false;
                return;
            }
            
            _rightHeart.DOFillAmount(0f, _duration);
        }
    }
}