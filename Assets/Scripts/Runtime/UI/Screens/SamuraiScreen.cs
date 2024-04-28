using System;
using Runtime.Infrastructure.Timer;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.UI.Screens
{
    public sealed class SamuraiScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timer;
        private IStopwatchable _stopwatchable;

        [Inject]
        private void Construct(IStopwatchable stopwatchable)
        {
            _stopwatchable = stopwatchable;
        }

        private void OnEnable()
        {
            _stopwatchable.Ticked += OnTicked;
        }

        private void OnDisable()
        {
            _stopwatchable.Ticked -= OnTicked;
        }

        private void OnTicked(int time)
        {
            _timer.text = time.ToString();
        }
    }
}