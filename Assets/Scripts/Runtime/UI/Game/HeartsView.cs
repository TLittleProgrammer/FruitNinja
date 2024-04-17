using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using UnityEngine;
using Zenject;

namespace Runtime.UI.Game
{
    public class HeartsView : MonoBehaviour, IAsyncInitializable<IUIFactory>
    {
        [SerializeField] private List<HeartView> _heartViews;

        private const string PathToHeartView = "Prefabs/UI/HealthView_01";
        private int _initialHealthCount;
        private int _lastHealth;

        [Inject]
        private void Construct(GameParameters gameParameters)
        {
            gameParameters.HealthChanged += OnChangedHealth;
            _initialHealthCount = gameParameters.Health;
            _lastHealth = _initialHealthCount;
        }
        
        public async UniTask AsyncInitialize(IUIFactory uiFactory)
        {
            int heartsCounter = _initialHealthCount / 2;

            for (int i = 0; i < heartsCounter; i++)
            {
                HeartView heartView = await uiFactory.LoadUIObjectByPath<HeartView>(PathToHeartView, transform);
                
                CorrectView(heartView);

                _heartViews.Add(heartView);
            }
        }

        private void OnChangedHealth(int health)
        {
            if (_lastHealth > health)
            {
                GetDamage(health);
            }
            else
            {
                GetHeel(health);
            }
        }

        private async void GetHeel(int health)
        {
            while (_lastHealth <= health)
            {
                int healthViewIndex = _lastHealth / 2 + (_lastHealth % 2 == 0 && _lastHealth != 0 ? -1 : 0);

                await _heartViews[^(healthViewIndex + 1)].AnimateGetHealth();
                _lastHealth++;
            }
        }

        private void GetDamage(int health)
        {
            int healthOffset = _initialHealthCount - health;

            int healthViewIndex = healthOffset / 2 + (healthOffset % 2 == 0 ? -1 : 0);
            _lastHealth = health;

            _heartViews[healthViewIndex].AnimateGetDamage();
        }

        private void CorrectView(HeartView heartView)
        {

            heartView.transform.localScale = Vector3.one;
        }
    }
}