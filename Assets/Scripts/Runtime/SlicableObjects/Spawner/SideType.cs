namespace Runtime.SlicableObjects.Spawner
{
    public enum SideType
    {
        None = 0,
        
        Left   = 1,
        Right  = 2,
        Bottom = 3,
    }

    public static class SideTypeExtension
    {
        public static float GetSidePosition(this SideType sideType, float ortographicSize)
        {
            return 1;
        }
    }
}