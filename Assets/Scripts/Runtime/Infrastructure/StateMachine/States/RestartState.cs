using Runtime.Infrastructure.Game;

namespace Runtime.Infrastructure.StateMachine.States
{
    public sealed class RestartState : IState
    {
        private readonly GameParameters _gameParameters;

        public RestartState(GameParameters gameParameters)
        {
            _gameParameters = gameParameters;
        }

        public void Enter()
        {
            _gameParameters.Reset();
        }

        public void Exit()
        {
            
        }
    }
}