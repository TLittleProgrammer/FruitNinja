using DG.Tweening;
using Runtime.Infrastructure.Timer;
using UnityEngine;
using Zenject;
using Sequence = DG.Tweening.Sequence;

namespace Runtime.Infrastructure.Effects
{
    public interface IAddictableFromScale
    {
        void OnTimeScaleChanged(float timeScale);
    }

    public class AddictableFromScale : MonoBehaviour, IAddictableFromScale
    {
        protected ITimeProvider TimeProvider;
        protected Sequence Sequence;

        [Inject]
        public void Construct(ITimeProvider timeProvider)
        {
            Sequence = DOTween.Sequence();
            TimeProvider = timeProvider;
        }

        private void OnEnable()
        {
            TimeProvider.TimeScaleChanged += OnTimeScaleChanged;
        }

        private void OnDisable()
        {
            TimeProvider.TimeScaleChanged -= OnTimeScaleChanged;
        }

        public void OnTimeScaleChanged(float timeScale)
        {
            Sequence.timeScale = timeScale;
        }
    }
}