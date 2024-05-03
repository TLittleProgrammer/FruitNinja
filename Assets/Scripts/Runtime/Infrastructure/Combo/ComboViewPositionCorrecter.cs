using Runtime.Infrastructure.Mouse;
using UnityEngine;

namespace Runtime.Infrastructure.Combo
{
    public interface IComboViewPositionCorrecter
    {
        void CorrectPosition(ComboView comboView, Vector2 position);
    }

    public sealed class ComboViewPositionCorrecter : IComboViewPositionCorrecter
    {
        private readonly MouseManager _manager;
        private readonly Vector2 _screenSize;
        private readonly Vector2 _originalSize = new(1366f, 768f);
        private Vector2 _multiplier;

        public ComboViewPositionCorrecter(MouseManager manager)
        {
            _manager = manager;
            _screenSize = new Vector2(Screen.width, Screen.height);
            _multiplier = new Vector2(_screenSize.x / _originalSize.x, _screenSize.y / _originalSize.y);
        }
        
        public void CorrectPosition(ComboView comboView, Vector2 position)
        {
            Vector2 viewportPosition = _manager.GetScreenPosition(position);
            Vector2 comboSize  = comboView.RectSize * _multiplier;
            Vector2 targetPosition = viewportPosition;
            
            if (viewportPosition.x + comboSize.x >= _screenSize.x)
            {
                targetPosition.x = _screenSize.x - comboSize.x * 1.1f;
            }
            else
            {
                if (viewportPosition.x - comboSize.x <= 0f)
                {
                    targetPosition.x = comboSize.x * 1.1f;
                }
            }
            
            if (viewportPosition.y + comboSize.y >= _screenSize.y)
            {
                targetPosition.y = _screenSize.y - comboSize.y * 1.1f;
            }
            else
            {
                if (viewportPosition.y - comboSize.y <= 0f)
                {
                    targetPosition.y = comboSize.y * 1.1f;
                }
            }
            
            comboView.SetPosition(targetPosition);
        }
    }
}