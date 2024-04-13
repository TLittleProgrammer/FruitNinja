using Runtime.Infrastructure.SlicableObjects.Spawner;

namespace Runtime.Extensions
{
    public static class SideTypeExtension
    {
        public static float GetAngle(this SideType sideType)
        {
            return sideType switch
            {
                SideType.Left  => -90f,
                SideType.Right => 90f,
                
                _ => 0f
            };
        }
    }
}