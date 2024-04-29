using System;
using Runtime.Infrastructure.StateMachine;

namespace Runtime.Infrastructure.Timer
{
    public interface IStopwatchable : IAsyncInitializable<IGameStateMachine>
    {
        event Action<int> Ticked;
        event Action TickEnded;

        void Notch(int time);
    }
}