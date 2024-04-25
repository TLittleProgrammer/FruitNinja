using Runtime.UI.Screens;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices.HealthFlying
{
    public interface IHealthFlyingService : IAsyncInitializable<GameScreen>
    {
        void Fly(Vector2 slicedPosition);
    }
}