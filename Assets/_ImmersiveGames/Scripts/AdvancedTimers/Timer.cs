using System;
using UnityEngine;

namespace _ImmersiveGames.Scripts.AdvancedTimers {
    public abstract class Timer : IDisposable {
        protected float CurrentTime { get; set; }
        public bool IsRunning { get; private set; }

        private float _initialTime;

        public float Progress => Mathf.Clamp(CurrentTime / _initialTime, 0, 1);

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float value) {
            _initialTime = value;
        }

        public void Start() {
            CurrentTime = _initialTime;
            if (IsRunning) return;
            IsRunning = true;
            TimerManager.RegisterTimer(this);
            OnTimerStart.Invoke();
        }

        public void Stop() {
            if (!IsRunning) return;
            IsRunning = false;
            TimerManager.DeregisterTimer(this);
            OnTimerStop.Invoke();
        }

        public abstract void Tick();
        public abstract bool IsFinished { get; }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        protected virtual void Reset() => CurrentTime = _initialTime;

        public virtual void Reset(float newTime) {
            _initialTime = newTime;
            Reset();
        }

        private bool _disposed;

        ~Timer() {
            Dispose(false);
        }

        // Call Dispose to ensure deregistration of the timer from the TimerManager
        // when the consumer is done with the timer or being destroyed
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (_disposed) return;

            if (disposing) {
                TimerManager.DeregisterTimer(this);
            }

            _disposed = true;
        }
    }
}