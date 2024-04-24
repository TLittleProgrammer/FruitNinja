using System;
using TMPro;

namespace Runtime.Infrastructure.DOTweenAnimationServices.Score
{
    public interface IScoreAnimationService : IDisposable
    {
        void Animate(TMP_Text text, int current, int target);
    }
}