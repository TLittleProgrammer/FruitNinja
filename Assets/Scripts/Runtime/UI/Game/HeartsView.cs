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

        [Inject]
        private void Construct(GameParameters gameParameters)
        {
            gameParameters.HealthChanged += OnChangedHealth;
            _initialHealthCount = gameParameters.Health;
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
            int healthOffset = _initialHealthCount - health;

            int healthViewIndex = healthOffset / 2 + (healthOffset % 2 == 0 ? -1 : 0);
  
            _heartViews[healthViewIndex].AnimateGetDamage();
        }

        private void CorrectView(HeartView heartView)
        {

            heartView.transform.localScale = Vector3.one;
        }
    }
}