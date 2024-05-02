using System.Collections.Generic;
using DG.Tweening;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.Timer;
using Runtime.StaticData.Boosts;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Services
{
    public interface IMimikService
    {
        void AddMimik(SlicableObjectView slicableObjectView);
        void RemoveMimik(SlicableObjectView slicableObjectView);
        SlicableObjectType GetRandomType();
        void SetSimulateSpeedToParticles(float simulateSpeed);
    }

    public class MimikService : IMimikService
    {
        private readonly Timer.Timer _timer;
        private readonly MimikSettings _mimikSettings;
        private readonly SlicableVisualContainer _slicableVisualContainer;
        private readonly Dictionary<SlicableObjectView, TimerData> _viewTimerMapping;

        public MimikService(Timer.Timer timer, MimikSettings mimikSettings, SlicableVisualContainer slicableVisualContainer, ITimeProvider timeProvider)
        {
            _timer = timer;
            _mimikSettings = mimikSettings;
            _slicableVisualContainer = slicableVisualContainer;
            _viewTimerMapping = new();

            timeProvider.TimeScaleChanged += OnTimeScaleChanged;
        }

        private void OnTimeScaleChanged(float timeScale)
        {
            foreach (var pair in _viewTimerMapping)
            {
                ParticleSystem.MainModule main = pair.Key.MagnetParticles.main;
                main.simulationSpeed = timeScale;
            }
        }

        public void AddMimik(SlicableObjectView slicableObjectView)
        {
            RemoveMimik(slicableObjectView);
            TimerData timerData = new();

            timerData.CurrentTime = timerData.InitialTime = _mimikSettings.Offset;
            timerData.SpecialIntervalBeforeShit = _mimikSettings.SpecialIntervalBeforeShit;
            timerData.TimeEnded = () =>
            {
                ChangeMimik(slicableObjectView);
            };
            timerData.ChangeScale = () =>
            {
                slicableObjectView
                    .transform
                    .DOScale(
                        new Vector3(_mimikSettings.Scale, _mimikSettings.Scale, _mimikSettings.Scale),
                        timerData.CurrentTime
                        )
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        timerData.IsAnimated = false;
                    });
            };
            
            _timer.AddTimerData(timerData);
            _viewTimerMapping.Add(slicableObjectView, timerData);
        }

        public void RemoveMimik(SlicableObjectView slicableObjectView)
        {
            if(_viewTimerMapping.ContainsKey(slicableObjectView))
            {
                _timer.RemoveTimeData(_viewTimerMapping[slicableObjectView]);
                _viewTimerMapping.Remove(slicableObjectView);
            }
        }

        public SlicableObjectType GetRandomType() =>
            _mimikSettings.AvailableTypes[Random.Range(0, _mimikSettings.AvailableTypes.Count)];

        public void SetSimulateSpeedToParticles(float simulateSpeed)
        {
            foreach (SlicableObjectView view in _viewTimerMapping.Keys)
            {
                ParticleSystem.MainModule main = view.MimikParticles.main;
                main.simulationSpeed = simulateSpeed;
            }
        }

        private void ChangeMimik(SlicableObjectView slicableObjectView)
        {
            slicableObjectView.SlicableObjectType = GetRandomType();
            
            Sprite sprite = _slicableVisualContainer.GetRandomSprite(slicableObjectView.SlicableObjectType);

            slicableObjectView.MainSprite.sprite = sprite;
            slicableObjectView.ShadowSprite.sprite = sprite;
        }
    }
}