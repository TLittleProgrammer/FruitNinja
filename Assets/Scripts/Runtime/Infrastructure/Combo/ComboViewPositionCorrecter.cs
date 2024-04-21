using Runtime.Infrastructure.Mouse;
using UnityEngine;

namespace Runtime.Infrastructure.Combo
{
    public interface IComboViewPositionCorrecter
    {
        void CorrectPosition(ComboView comboView);
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
        
        //TODO доработать логику. Не работает
        public void CorrectPosition(ComboView comboView)
        {
            Vector2 viewportPosition = _manager.GetViewportPosition(comboView.transform.position);

            if (viewportPosition.x < 0.15f)
            {
                viewportPosition.x = 0.15f;
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

            comboView.transform.position = _manager.GetWorldPositionByViewport(viewportPosition);
        }
    }
}