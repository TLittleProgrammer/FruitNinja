using System.Linq;
using Runtime.Extensions;
using Runtime.Infrastructure.Combo;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.Score;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class SimpleSliceService : ISliceService
    {
        private readonly SlicableVisualContainer _slicableVisualContainer;
        private readonly SlicableMovementService _slicableMovementService;
        private readonly IShowEffectsService _showEffectsService;
        private readonly IAddScoreService _addScoreService;
        private readonly IComboService _comboService;
        private readonly ICreateDummiesService _createDummiesService;

        private Vector2 _lastSlicedPosition;
        
        public SimpleSliceService(
            SlicableVisualContainer slicableVisualContainer,
            SlicableMovementService slicableMovementService,
            IShowEffectsService showEffectsService,
            IAddScoreService addScoreService,
            IComboService comboService,
            ICreateDummiesService createDummiesService
        )
        {
            _slicableVisualContainer = slicableVisualContainer;
            _slicableMovementService = slicableMovementService;
            _showEffectsService = showEffectsService;
            _addScoreService = addScoreService;
            _comboService = comboService;
            _createDummiesService = createDummiesService;

            _addScoreService.AddedScore += OnAddedScore;
        }
        
        public bool TrySlice(SlicableObjectView slicableObjectView)
        {
            Sprite slicableObjectSprite = slicableObjectView.MainSprite.sprite;
            Sprite sprite = _slicableVisualContainer.GetSlicedSpriteByName(slicableObjectSprite.name);
            _lastSlicedPosition = slicableObjectView.transform.position;

            _createDummiesService.AddDummies(slicableObjectView, sprite, slicableObjectSprite);
            RemoveSlicableObjectFromMapping(slicableObjectView);

            _addScoreService.Add();
            _showEffectsService.ShowSplash(_lastSlicedPosition, slicableObjectSprite);
            _showEffectsService.ShowBlots(_lastSlicedPosition, slicableObjectSprite);

            _comboService.AddCombo();

            return true;
        }

        private void OnAddedScore(int score)
        {
            _showEffectsService.ShowScore(_lastSlicedPosition, score);
        }

        private void RemoveSlicableObjectFromMapping(SlicableObjectView slicableObjectView)
        {
            _slicableMovementService.RemoveFromMapping(slicableObjectView.transform);
            slicableObjectView.gameObject.SetActive(false);
        }
    }
}