using Runtime.Extensions;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.StaticData.Boosts;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices.Helpers
{
    public sealed class SplashBombService : ISplashBombService
    {
        private readonly BombSettings _bombSettings;
        private readonly SlicableMovementService _slicableMovementService;

        public SplashBombService(BombSettings bombSettings, SlicableMovementService slicableMovementService)
        {
            _bombSettings = bombSettings;
            _slicableMovementService = slicableMovementService;
        }
        
        public void Boom(Vector2 bombPosition)
        {
            foreach (SlicableModel model in _slicableMovementService.SlicableModels)
            {
                //TODO По хорошему будет сравнивать сумму квадратов, не считая корень
                float distance = Vector2.Distance(model.Position, bombPosition);

                if (distance <= _bombSettings.Radius)
                {
                    float force = Mathf.Lerp(_bombSettings.MaxForce, 0f, distance / _bombSettings.Radius);
                    Vector2 direction = model.Position - bombPosition;

                    float sign = (direction.y >= 0 ? 1 : -1);
                    float offset = (sign >= 0 ? 0 : 360f);
                    float angle = Vector2.Angle(Vector2.right, direction) * sign + offset;
                    
                    model.ResetMovementObjectService(force, force, angle.ConvertToRadians(), model.Position);
                }
            }
        }
    }
}