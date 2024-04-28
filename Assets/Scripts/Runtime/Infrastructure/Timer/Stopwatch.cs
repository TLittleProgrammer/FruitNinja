using System;
using Cysharp.Threading.Tasks;

namespace Runtime.Infrastructure.Timer
{
    public sealed class Stopwatch : IStopwatchable
    {
        public event Action<int> Ticked;
        public event Action TickEnded;

        public async void Notch(int time)
        {
            Ticked?.Invoke(time);
            
            while (time > 0)
            {
                await UniTask.Delay(1000);
                time--;
                Ticked?.Invoke(time);
            }
            
            TickEnded?.Invoke();
        }
    }
}