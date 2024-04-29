using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.Trail;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class BrickSliceService : ISliceService
    {
        private readonly TrailMoveService _trailMoveService;
        private readonly MouseManager _mouseManager;
        private readonly IShowEffectsService _showEffectsService;

        public BrickSliceService(TrailMoveService trailMoveService, MouseManager mouseManager, IShowEffectsService showEffectsService)
        {
            _trailMoveService = trailMoveService;
            _mouseManager = mouseManager;
            _showEffectsService = showEffectsService;
        }

        public bool TrySlice(SlicableObjectView slicableObjectView)
        {
            _trailMoveService.SetCannotMove();
            _mouseManager.SetCannotMouseCheckPosition();
            _showEffectsService.ShowSplash(slicableObjectView.transform.position, slicableObjectView.MainSprite.sprite.name);
            
            return false;
        }
    }
}