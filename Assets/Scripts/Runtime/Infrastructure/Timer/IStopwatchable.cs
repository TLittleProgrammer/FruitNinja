using System;
using Runtime.Infrastructure.StateMachine;
using Zenject;

namespace Runtime.Infrastructure.Timer
{
    public interface IStopwatchable : IAsyncInitializable<IGameStateMachine, ITimeProvider>, ITickable
    {
        event Action<int> Ticked;
        event Action TickEnded;

        void Notch(int time);
    }
}