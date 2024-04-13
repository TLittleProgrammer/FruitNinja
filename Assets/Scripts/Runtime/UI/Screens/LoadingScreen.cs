using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.StaticData.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.UI.Screens
{
    public sealed class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private readonly Color _transparent = Color.black;
        private float _showAndHideDuration;

        [Inject]
        public void Construct(LoadingScreenFadeDuration loadingScreenFadeDuration)
        {
            _showAndHideDuration = loadingScreenFadeDuration.Duration;
        }

        public async UniTask Show()
        {
            gameObject.SetActive(true);

            await _image.DOColor(Color.white, _showAndHideDuration).ToUniTask();
        }

        public void Hide()
        {
            _image
                .DOColor(_transparent, _showAndHideDuration)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}