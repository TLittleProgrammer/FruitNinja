using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Loose
{
    public sealed class LooseService : ILooseService
    {
        private readonly Canvas _looseScreenParent;
        private readonly IUIFactory _uiFactory;
        private readonly DiContainer _diContainer;
        private readonly UserData.UserData _userData;

        public LooseService(
            Canvas looseScreenParent,
            IUIFactory uiFactory,
            GameParameters gameParameters,
            DiContainer diContainer
            )
        {
            _looseScreenParent = looseScreenParent;
            _uiFactory = uiFactory;
            _diContainer = diContainer;

            gameParameters.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(int health)
        {
            if (health == 0)
            {
                _uiFactory.LoadScreen<LooseScreen>(ScreenType.Loose, _looseScreenParent.transform, _diContainer);
            }
        }
    }
}