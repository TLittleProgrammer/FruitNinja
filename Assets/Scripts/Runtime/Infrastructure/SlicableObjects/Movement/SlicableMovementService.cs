using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.SlicableObjects.Movement
{
    public class SlicableMovementService : ITickable
    {
        private SliceableMapping _slicableMapping;

        public SlicableMovementService()
        {
            _slicableMapping = new();
        }
        
        public void Tick()
        {
            foreach (SlicableModel model in _slicableMapping.Values)
            {
                model.Tick();
            }
        }

        public void AddMapping(SlicableModel model, Transform view)
        {
            _slicableMapping.Add(view, model);
        }

        public void RemoveFromMapping(Transform view)
        {
            _slicableMapping.Remove(view);
        }

        public SlicableModel GetSliceableModel(Transform view)
        {
            return _slicableMapping.TryGetValue(view, out SlicableModel slicableModel) ? slicableModel : null;
        }
    }

    public sealed class SliceableMapping : Dictionary<Transform, SlicableModel>
    {
        
    }
}