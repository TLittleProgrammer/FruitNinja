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

        public ComboViewPositionCorrecter(
                MouseManager manager
            )
        {
            _manager = manager;
        }
        
        public void CorrectPosition(ComboView comboView, Vector2 position)
        {
            Vector2 viewportPosition = _manager.GetViewportPosition(position);

            if (viewportPosition.x < 0.25f)
            {
                viewportPosition.x = 0.25f;
            }
            else
            {
                if (viewportPosition.x > 0.75f)
                {
                    viewportPosition.x = 0.75f;
                }
            }
            
            
            if (viewportPosition.y < 0.15f)
            {
                viewportPosition.y = 0.15f;
            }
            else
            {
                if (viewportPosition.y > 0.75f)
                {
                    viewportPosition.y = 0.75f;
                }
            }

            comboView.SetPosition(_manager.GetScreenPositionByViewport(viewportPosition));
        }
    }
}