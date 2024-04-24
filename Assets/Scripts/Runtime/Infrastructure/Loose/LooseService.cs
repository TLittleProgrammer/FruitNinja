using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;

namespace Runtime.Infrastructure.Loose
{
    public sealed class LooseService : ILooseService
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly UserData.UserData _userData;
        
        public LooseService(GameParameters gameParameters, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;

            gameParameters.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(int health)
        {
            if (health <= 0 && _gameStateMachine.CurrentState is not LooseState)
            {
                _gameStateMachine.Enter<LooseState>();
            }
        }
    }
}