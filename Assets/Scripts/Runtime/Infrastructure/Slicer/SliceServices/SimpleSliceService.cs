using Runtime.Infrastructure.Combo;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Score;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class SimpleSliceService : SliceService
    {
        private readonly IShowEffectsService _showEffectsService;
        private readonly IAddScoreService _addScoreService;
        private readonly IComboService _comboService;
        private readonly ICreateDummiesService _createDummiesService;

        private Vector2 _lastSlicedPosition;
        
        public SimpleSliceService(SlicableMovementService slicableMovementService,
            IShowEffectsService showEffectsService,
            IAddScoreService addScoreService,
            IComboService comboService,
            ICreateDummiesService createDummiesService) : base(slicableMovementService)
        {
            _showEffectsService = showEffectsService;
            _addScoreService = addScoreService;
            _comboService = comboService;
            _createDummiesService = createDummiesService;

            _addScoreService.AddedScore += OnAddedScore;
        }
        
        public override bool TrySlice(SlicableObjectView slicableObjectView)
        {
            string mainSpriteName = slicableObjectView.MainSprite.sprite.name;
            _lastSlicedPosition = slicableObjectView.transform.position;

            _createDummiesService.AddDummies(slicableObjectView);
            RemoveSlicableObjectFromMapping(slicableObjectView);

            _addScoreService.Add();
            _showEffectsService.ShowSplash(_lastSlicedPosition, mainSpriteName);
            _showEffectsService.ShowBlots(_lastSlicedPosition, mainSpriteName);

            _comboService.AddCombo();

            return true;
        }

        private void OnAddedScore(int score)
        {
            _showEffectsService.ShowScore(_lastSlicedPosition, score);
        }
    }
}