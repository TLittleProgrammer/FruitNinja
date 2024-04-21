using System;
using Runtime.Infrastructure.Combo;
using Runtime.Infrastructure.Game;
using Random = UnityEngine.Random;

namespace Runtime.Infrastructure.Score
{
    public interface IAddScoreService
    {
        event Action<int> AddedScore;
        void Add();
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

            comboService.ComboEnded += OnComboEnded;
        }

        public event Action<int> AddedScore;

        public void Add()
        {
            int score;

            if (_comboService.IsActive)
            {
                score = _lastScore;
            }
            else
            {
                score = Random.Range(25, 100);
                _lastScore = score;
            }
            
            ChangeScore(score);
        }

        private void OnComboEnded(int comboCounter)
        {
            int score = comboCounter * _lastScore * (comboCounter - 1);

            ChangeScore(score);
        }

        private void ChangeScore(int score)
        {
            _gameParameters.ChangeScore(score);
            AddedScore?.Invoke(score);
        }
    }
}