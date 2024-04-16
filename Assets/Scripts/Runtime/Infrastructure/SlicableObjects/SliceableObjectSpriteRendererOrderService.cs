using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects
{
    public sealed class SliceableObjectSpriteRendererOrderService
    {
        private const int MaxOrders = 1000;
        private int _currentOrder = 0;

        public void UpdateOrderInLayer(SpriteRenderer spriteRenderer)
        {
            if (_currentOrder >= MaxOrders)
            {
                _currentOrder = 0;
            }

            spriteRenderer.sortingOrder = _currentOrder;

            _currentOrder++;
        }
    }
}