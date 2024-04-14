using System;
using Runtime.Infrastructure.Game;

namespace Runtime.Infrastructure.UserData
{
    public class UserData
    {
        public int BestScore { get; private set; }

        public event Action<int> BestScoreChanged;
        
        public UserData(GameParameters gameParameters)
        {
            BestScore = 0;
            
            //TODO подойдет ли такое именоавние?
            gameParameters.ScoreChanged += SetNewBestScore;
        }

        public void SetNewBestScore(int newBestScore)
        {
            if (newBestScore > BestScore)
            {
                BestScore = newBestScore;
                
                BestScoreChanged?.Invoke(BestScore);
            }
        }
    }
}