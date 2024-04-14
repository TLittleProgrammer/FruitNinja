using System;

namespace Runtime.Infrastructure.UserData
{
    public class UserData
    {
        public int BestScore { get; private set; }

        public event Action<int> BestScoreChanged;
        
        public UserData()
        {
            BestScore = 0;
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