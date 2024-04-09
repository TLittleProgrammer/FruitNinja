using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Screens
{
    public sealed class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private readonly Color _transparent = new(0f, 0f, 0f, 0f);
        private float _showAndHideDuration = 1.5f;

        public async UniTask Show()
        {
            gameObject.SetActive(true);

            _image.DOColor(Color.white, _showAndHideDuration);

            await UniTask.Delay((int)_showAndHideDuration * 1000);
        }

        public void Hide()
        {
            _image
                .DOColor(_transparent, _showAndHideDuration)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}