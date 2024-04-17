using Cysharp.Threading.Tasks;
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

        private int _health = 2;
        
        public async void AnimateGetDamage()
        {
            if (_health == 2)
            {
                await AnimateHeart(_leftHeart, 0f, _duration, -1);
                return;
            }

            await AnimateHeart(_rightHeart, 0f, _duration, -1);
        }

        public async UniTask AnimateGetHealth()
        {
            if (_health == 0)
            {
                Debug.Log("A");
                await AnimateHeart(_rightHeart, 1f, _duration / 2f, 1);
                _health++;
            }
            else
            {
                await AnimateHeart(_leftHeart, 1f, _duration / 2f, 1);
            }
        }

        private async UniTask AnimateHeart(Image image, float endValue, float duration, int value)
        {
            await image.DOFillAmount(endValue, duration).ToUniTask();
            _health += value;
        }
    }
}