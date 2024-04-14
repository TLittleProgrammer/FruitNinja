using System;

namespace Runtime.Infrastructure.UserData
{
    [Serializable]
    public class UserData
    {
        public int bestScore;

        public event Action<int> BestScoreChanged;
        public event Action UserDataWasUpdated;
        
        public UserData()
        {
            bestScore = 0;
        }

        public void SetNewBestScore(int newBestScore)
        {
            if (newBestScore > bestScore)
            {
                bestScore = newBestScore;

                UserDataWasUpdated?.Invoke();
                BestScoreChanged?.Invoke(bestScore);
            }
        }

        public void UpdateParams(UserData deserializedUserData)
        {
            bestScore = deserializedUserData.bestScore;
            
            UserDataWasUpdated?.Invoke();
            BestScoreChanged?.Invoke(bestScore);
        }
    }
}