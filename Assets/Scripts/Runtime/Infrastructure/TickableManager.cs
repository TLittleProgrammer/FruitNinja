using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure
{
    //TODO Удалить?
    public class TickableManager : MonoBehaviour
    {
        private List<ITickable> _tickableList = new();

        private void Update()
        {
            foreach (ITickable tickable in _tickableList)
            {
                tickable.Tick();
            }
        }

        public void SetTickable(ITickable tickable)
        {
            _tickableList.Add(tickable);
        }
    }
}