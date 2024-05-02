using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.Slicer.SliceServices;

namespace Runtime.Infrastructure.StateMachine.States
{
    public sealed class RestartState : IState
    {
        private readonly GameParameters _gameParameters;
        private readonly MagnetSliceService _magnetSliceService;

        public RestartState(GameParameters gameParameters, MagnetSliceService magnetSliceService)
        {
            _gameParameters = gameParameters;
            _magnetSliceService = magnetSliceService;
        }

        public void Enter()
        {
            _gameParameters.Reset();
            _magnetSliceService.ResetAll();
        }

        public void Exit()
        {
            
        }
    }
}