using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.SlicableObjects.Movement
{
    public class SlicableMovementService : ITickable
    {
        private SliceableMapping _slicableMapping;
        private bool _canMove;
        
        public SlicableMovementService()
        {
            _slicableMapping = new();
            _canMove = true;
        }

        public int MovementObjectsCount => _slicableMapping.Count;

        public void Tick()
        {
            if (_canMove is false)
                return;
            
            foreach (SlicableModel model in _slicableMapping.Values)
            {
                model.Tick();
            }
        }

        public void Reset()
        {
            _canMove = true;

            foreach (Transform transform in _slicableMapping.Keys)
            {
                transform.gameObject.SetActive(false);
            }
            
            _slicableMapping.Clear();
        }
        
        public void Stop()
        {
            _canMove = false;
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