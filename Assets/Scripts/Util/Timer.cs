using System;

namespace Util
{
    public class Timer
    {
        public float ElapsedSeconds { get; private set; }
        public float TimeScale { get; set; }
        private float m_time;
        public bool IsRunning { get; private set; } = false;

        private Action m_onFinished;
    
        public Timer(float timeSeconds, Action onFinished, float timeScale = 1.0f)
        {
            m_time = timeSeconds;
            m_onFinished = onFinished;
            TimeScale = timeScale;
        }

        public Timer(float timeSeconds) : this(timeSeconds, () => { })
        {
        
        }

        public bool IsFinished() => ElapsedSeconds >= m_time;

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Reset()
        {
            Stop();
            ElapsedSeconds = 0.0f;
        }

        public void Restart()
        {
            Reset();
            Start();
        }

        public void Update(float deltaTime)
        {
            if (!IsRunning) return;
        
            ElapsedSeconds += TimeScale * deltaTime;

            if (IsFinished())
            {
                Stop();
                m_onFinished.Invoke();
            }
        }
    }
}
