using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class BombSliceService : SliceService
    {
        private readonly ICreateDummiesService _createDummiesService;
        private readonly IShowEffectsService _showEffectsService;
        private readonly GameParameters _gameParameters;
        private readonly ISplashBombService _splashBombService;

        public BombSliceService(
            SlicableMovementService slicableMovementService,
            ICreateDummiesService createDummiesService,
            IShowEffectsService showEffectsService,
            GameParameters gameParameters,
            ISplashBombService splashBombService) : base(slicableMovementService)
        {
            _createDummiesService = createDummiesService;
            _showEffectsService = showEffectsService;
            _gameParameters = gameParameters;
            _splashBombService = splashBombService;
        }
        
        public override bool TrySlice(SlicableObjectView slicableObjectView)
        {
            _createDummiesService.AddDummies(slicableObjectView);
            RemoveSlicableObjectFromMapping(slicableObjectView);
            _gameParameters.ChangeHealth(-1);
            _splashBombService.Boom(slicableObjectView.transform.position);

            _showEffectsService.PlayBombEffect(slicableObjectView.transform.position);
            return true;
        }
    }
}