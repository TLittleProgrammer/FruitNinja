using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.EntryPoint;
using Runtime.StaticData.Blur;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.UI.Screens
{
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private BlurEffect _blurEffect;
        [SerializeField] private BlurSettings _blurSettings;
        
        private IEntryPoint _entryPoint;

        [Inject]
        private void Construct(IEntryPoint entryPoint)
        {
            _entryPoint = entryPoint;
            _blurEffect.Initialize(_blurSettings.InitialSize);
        }

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            _quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }

        private async void OnPlayButtonClicked()
        {
            Destroy(_playButton);
            Destroy(_quitButton);
            
            await _blurEffect.UpdateBlur(_blurSettings.Target, _blurSettings.Duration);
            
            _entryPoint.AsyncLoadScene(Constants.SceneNames.Game);
        }

        private void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}