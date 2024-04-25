using System;
using DG.Tweening;
using Runtime.StaticData.UI;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Runtime.Infrastructure.Slicer.SliceServices.HealthFlying
{
    public class FlyingHealth
    {
        public readonly FlyingHealthView FlyingHealthView;
        public Vector2 TargetPosition;

        private readonly FlyingHealthViewStaticData _flyingHealthViewStaticData;

        private Sequence _sequence;
        private Action _flyEnded;

        public FlyingHealth(FlyingHealthView flyingHealthView, FlyingHealthViewStaticData flyingHealthViewStaticData)
        {
            FlyingHealthView = flyingHealthView;
            _flyingHealthViewStaticData = flyingHealthViewStaticData;
            _sequence = DOTween.Sequence();
        }

        public void FlyTo(Vector2 target, Action flyEnded)
        {
            FlyingHealthView.gameObject.SetActive(true);
            FlyingHealthView.transform.localScale = Vector3.one;
            
            TargetPosition = target;
            _flyEnded = flyEnded;
            
            _sequence.Kill();
            _sequence = DOTween.Sequence();

            _sequence.Append(
                FlyingHealthView
                    .RectTransform
                    .DOMove(target, _flyingHealthViewStaticData.FlyDuration)
                    .SetDelay(_flyingHealthViewStaticData.TimeDelayAfterAnimation)
            );

            _sequence.OnComplete(() =>
            {
                flyEnded.Invoke();
                FlyingHealthView.gameObject.SetActive(false);
                _sequence.Kill();
            });
        }

        public void ChangeTargetPosition(Vector2 targetPosition)
        {
            FlyTo(targetPosition, _flyEnded);
        }

        public void DestroyView()
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();

            _sequence.Append(FlyingHealthView.transform.DOScale(Vector3.zero, _flyingHealthViewStaticData.DestroyDuration));
            _sequence.OnComplete(() =>
            {
                FlyingHealthView.gameObject.SetActive(false);
                _sequence.Kill();
            });
        }
    }
}