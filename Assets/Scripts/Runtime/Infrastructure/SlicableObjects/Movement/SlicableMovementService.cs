using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.SlicableObjects.Movement
{
    public class SlicableMovementService : ITickable
    {
        private List<(SlicableModel, Transform)> _slicableModels;

        public SlicableMovementService()
        {
            _slicableModels = new();
        }
        
        public void Tick()
        {
            foreach ((SlicableModel model, Transform view) in _slicableModels)
            {
                model.SimulateMoving();
                model.SimulateRotating();
                
                view.transform.position = model.Position;
                view.transform.rotation = model.Rotation;
            }
        }

        public void AddMapping(SlicableModel model, Transform view)
        {
            _slicableModels.Add((model, view));
        }

        public void RemoveFromMapping(Transform view)
        {
            (SlicableModel, Transform) mapping = _slicableModels.First(_ => EqualsTransforms(_.Item2, view));
            
            _slicableModels.Remove(mapping);
        }

        public SlicableModel GetSliceableModel(Transform view)
        {
            return _slicableModels.First(_ => EqualsTransforms(_.Item2, view)).Item1;
        }

        private bool EqualsTransforms(Transform transform, Transform view)
        {
            return transform.Equals(view);
        }
    }
}