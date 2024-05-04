using Runtime.UI.Screens;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices.HealthFlying
{
    public interface IHealthFlyingService : IAsyncInitializable<GameScreen>
    {
        int HealthCounter { get; }
        
        void Fly(Vector2 slicedPosition, Vector3 transformLocalScale);
    }
}