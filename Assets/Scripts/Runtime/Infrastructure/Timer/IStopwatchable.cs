using System;

namespace Runtime.Infrastructure.Timer
{
    public interface IStopwatchable
    {
        event Action<int> Ticked;
        event Action TickEnded;

        void Notch(int time);
    }
}