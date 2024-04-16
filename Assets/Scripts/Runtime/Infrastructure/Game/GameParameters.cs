using System;
using Runtime.StaticData.Level;

namespace Runtime.Infrastructure.Game
{
    public class GameParameters
    {
        private readonly UserData.UserData _userData;
        private int _health;
        private int _currentScore = 0;
        
        public event Action<int> HealthChanged;
        public event Action<int> ScoreChanged;

        public GameParameters(LevelStaticData levelStaticData, UserData.UserData userData)
        {
            _userData = userData;
            _health = levelStaticData.HealthCount * 2;
        }

        public int Health => _health;

        public void ChangeScore(int addScore)
        {
            _currentScore += addScore;

            ScoreChanged?.Invoke(_currentScore);

            if (_userData.bestScore < _currentScore)
            {
                _userData.SetNewBestScore(_currentScore);
            }
        }

        public void ChangeHealth(int count)
        {
            _health += count;

            if (_health < 0)
            {
                _health = 0;
            }
            
            HealthChanged?.Invoke(_health);
        }

        public void Reset()
        {
            _health = Constants.Game.InitialHealthCount;
            _currentScore = 0;
            
            ScoreChanged?.Invoke(_currentScore);
            HealthChanged?.Invoke(_health);
        }
    }
}