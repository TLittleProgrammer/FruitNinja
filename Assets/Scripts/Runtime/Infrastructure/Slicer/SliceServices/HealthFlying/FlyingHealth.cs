using System;
using DG.Tweening;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
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

        public Sequence MoveSequence;
        public Tween ScaleSequence;
        private Action _flyEnded;
        private Vector2 _toRect = new(70f, 62f);
        private Vector2 _initialSize;

        public FlyingHealth(FlyingHealthView flyingHealthView, FlyingHealthViewStaticData flyingHealthViewStaticData)
        {
            FlyingHealthView = flyingHealthView;
            _flyingHealthViewStaticData = flyingHealthViewStaticData;
            MoveSequence = DOTween.Sequence();
        }

        public void FlyTo(Vector2 target, Action flyEnded)
        {
            _initialSize = FlyingHealthView.ImageRect.rect.size;
            FlyingHealthView.gameObject.SetActive(true);
            FlyingHealthView.transform.localScale = Vector3.one;
            
            TargetPosition = target;
            _flyEnded = flyEnded;
            
            MoveSequence.Kill();
            MoveSequence = DOTween.Sequence();

            ScaleSequence?.Kill();
            ScaleSequence = FlyingHealthView.ImageRect.DOSizeDelta(_toRect, _flyingHealthViewStaticData.FlyDuration);
            
            MoveSequence.Append(
                FlyingHealthView
                    .RectTransform
                    .DOMove(target, _flyingHealthViewStaticData.FlyDuration)
                    .SetDelay(_flyingHealthViewStaticData.TimeDelayAfterAnimation)
            );
            

            MoveSequence.OnComplete(() =>
            {
                flyEnded.Invoke();
                FlyingHealthView.gameObject.SetActive(false);
                MoveSequence.Kill();
            });
        }

        public void ChangeTargetPosition(Vector2 targetPosition)
        {
            FlyTo(targetPosition, _flyEnded);
        }

        public void DestroyView(Action destroyEnded)
        {
            MoveSequence.Kill();
            MoveSequence = DOTween.Sequence();

            MoveSequence.Append(FlyingHealthView.transform.DOScale(Vector3.zero, _flyingHealthViewStaticData.DestroyDuration));
            MoveSequence.OnComplete(() =>
            {
                FlyingHealthView.gameObject.SetActive(false);
                destroyEnded.Invoke();
                MoveSequence.Kill();
            });
        }
    }
}