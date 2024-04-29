using System.Collections.Generic;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.Timer;
using Runtime.StaticData.Boosts;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Services
{
    public interface IMimikService
    {
        void AddMimik(SlicableObjectView slicableObjectView);
        void RemoveMimik(SlicableObjectView slicableObjectView);
        SlicableObjectType GetRandomType();

    }

    public class MimikService : IMimikService
    {
        private readonly Timer.Timer _timer;
        private readonly MimikSettings _mimikSettings;
        private readonly SlicableVisualContainer _slicableVisualContainer;
        private readonly Dictionary<SlicableObjectView, TimerData> _viewTimerMapping;

        public MimikService(Timer.Timer timer, MimikSettings mimikSettings, SlicableVisualContainer slicableVisualContainer)
        {
            _timer = timer;
            _mimikSettings = mimikSettings;
            _slicableVisualContainer = slicableVisualContainer;
            _viewTimerMapping = new();
        }

        public void AddMimik(SlicableObjectView slicableObjectView)
        {
            RemoveMimik(slicableObjectView);
            TimerData timerData = new();

            timerData.CurrentTime = timerData.InitialTime = _mimikSettings.Offset;
            timerData.TimeEnded = () =>
            {
                ChangeMimik(slicableObjectView);
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

        private void ChangeMimik(SlicableObjectView slicableObjectView)
        {
           
            slicableObjectView.SlicableObjectType = GetRandomType();
            
            Sprite sprite = _slicableVisualContainer.GetRandomSprite(slicableObjectView.SlicableObjectType);

            slicableObjectView.MainSprite.sprite = sprite;
            slicableObjectView.ShadowSprite.sprite = sprite;
        }
    }
}