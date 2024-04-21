using Runtime.Infrastructure.Combo;
using Runtime.Infrastructure.Game;
using Random = UnityEngine.Random;

namespace Runtime.Infrastructure.Score
{
    public interface IAddScoreService
    {
        int Add();
    }

    public sealed class AddScoreService : IAddScoreService
    {
        private readonly GameParameters _gameParameters;
        private readonly IComboService _comboService;

        private int _lastScore;

        public AddScoreService(
            GameParameters gameParameters,
            IComboService comboService
            )
        {
            _gameParameters = gameParameters;
            _comboService = comboService;
            _lastScore = 0;
        }

        public int Add()
        {
            int score = _comboService.IsActive ? _lastScore : Random.Range(25, 100);
            
            _gameParameters.ChangeScore(score);

            return score;
        }
    }
}