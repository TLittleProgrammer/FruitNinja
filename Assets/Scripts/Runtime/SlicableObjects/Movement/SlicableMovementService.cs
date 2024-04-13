using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Runtime.SlicableObjects.Movement
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
    }
}