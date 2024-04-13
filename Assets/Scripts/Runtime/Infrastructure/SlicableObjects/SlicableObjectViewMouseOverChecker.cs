using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.SlicableObjects
{
    [RequireComponent(typeof(SlicableObjectView))]
    public class SlicableObjectViewMouseOverChecker : MonoBehaviour
    {
        private SlicableObjectView _slicableObjectView;
        private CanSliceResolver _canSliceResolver;

        [Inject]
        private void Construct(CanSliceResolver canSliceResolver)
        {
            _canSliceResolver = canSliceResolver;
        }
        
        private void Awake()
        {
            _slicableObjectView = GetComponent<SlicableObjectView>();
        }

        private void OnMouseOver()
        {
            _canSliceResolver.TrySlice(_slicableObjectView);
        }
    }
}