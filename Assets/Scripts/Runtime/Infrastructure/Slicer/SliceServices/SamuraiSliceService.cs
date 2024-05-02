using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class SamuraiSliceService : SliceService
    {
        private readonly ICreateDummiesService _createDummiesService;
        private readonly IGameStateMachine _stateMachine;

        public SamuraiSliceService(
            SlicableMovementService slicableMovementService,
            ICreateDummiesService createDummiesService,
            IGameStateMachine stateMachine) : base(slicableMovementService)
        {
            _createDummiesService = createDummiesService;
            _stateMachine = stateMachine;
        }

        public override bool TrySlice(SlicableObjectView slicableObjectView)
        {
            _stateMachine.Enter<SamuraiState>();
            _createDummiesService.AddDummies(slicableObjectView);
            RemoveSlicableObjectFromMapping(slicableObjectView);
            return true;
        }
    }
}