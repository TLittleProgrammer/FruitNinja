using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.CollisionDetector;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;
using Runtime.StaticData.Boosts;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public class AvosjkaSliceService : SliceService
    {
        private readonly SlicableModelViewMapper _slicableModelViewMapper;
        private readonly ICreateDummiesService _createDummiesService;
        private readonly AvosjkaSettings _avosjkaSettings;
        private readonly ICollisionDetector<Collider2D, SlicableObjectView> _collisionDetector;

        public AvosjkaSliceService(
            SlicableMovementService slicableMovementService,
            SlicableModelViewMapper slicableModelViewMapper,
            ICreateDummiesService createDummiesService,
            AvosjkaSettings avosjkaSettings,
            ICollisionDetector<Collider2D, SlicableObjectView> collisionDetector) : base(slicableMovementService)
        {
            _slicableModelViewMapper = slicableModelViewMapper;
            _createDummiesService = createDummiesService;
            _avosjkaSettings = avosjkaSettings;
            _collisionDetector = collisionDetector;
        }

        public override bool TrySlice(SlicableObjectView slicableObjectView)
        {
            List<SlicableObjectView> slicableObjectViews = new(); 
            
            for (int i = 0; i < _avosjkaSettings.PackSize; i++)
            {
                var x= _slicableModelViewMapper.AddMappingWithoutCollisonDetector(
                    SlicableObjectType.Simple,
                    slicableObjectView.transform.position,
                    new(125f, 75f),
                    new(10f, 5f),
                    new(10f, 5f)
                );
                
                slicableObjectViews.Add(x);
            }
            
            _createDummiesService.AddDummies(slicableObjectView, "avosjka_01", "avosjka_02");
            RemoveSlicableObjectFromMapping(slicableObjectView);

            Delay(slicableObjectViews);
            
            return true;
        }

        private async void Delay(List<SlicableObjectView> slicableObjectViews)
        {
           await UniTask.WaitForSeconds(_avosjkaSettings.Invulnerability);

           foreach (SlicableObjectView view in slicableObjectViews)
           {
               _collisionDetector.AddCollider(view.Collider2D, view);
           }
        }
    }
}