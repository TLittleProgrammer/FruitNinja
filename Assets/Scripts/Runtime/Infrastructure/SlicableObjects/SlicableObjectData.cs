namespace Runtime.Infrastructure.SlicableObjects
{
    public class SlicableObjectData
    {
        public SlicableObjectType Type;
        public SlicableObjectView View;

        public SlicableObjectData(SlicableObjectType type, SlicableObjectView view)
        {
            Type = type;
            View = view;
        }
    }
}