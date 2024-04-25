using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.Slicer.SliceServices.HealthFlying;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;

namespace Runtime.Infrastructure.Loose
{
    public sealed class LooseService : ILooseService
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IHealthFlyingService _healthFlyingService;
        private readonly UserData.UserData _userData;
        
        public LooseService(GameParameters gameParameters, IGameStateMachine gameStateMachine, IHealthFlyingService healthFlyingService)
        {
            _gameStateMachine = gameStateMachine;
            _healthFlyingService = healthFlyingService;

            gameParameters.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(int health)
        {
            if (health + _healthFlyingService.HealthCounter <= 0 && _gameStateMachine.CurrentState is not LooseState)
            {
                _gameStateMachine.Enter<LooseState>();
            }
        }
    }
}