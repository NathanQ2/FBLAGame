using System;
using System.Diagnostics;
using UnityEngine;

public class Timer
{
    public float elapsedSeconds { get; private set; }
    private float m_time;
    public bool isRunning { get; private set; } = false;

    private Action m_onFinished;
    
    public Timer(float timeSeconds, Action onFinished)
    {
        m_time = timeSeconds;
        m_onFinished = onFinished;
    }

    public Timer(float timeSeconds) : this(timeSeconds, () => { })
    {
        
    }

    public bool IsFinished() => elapsedSeconds >= m_time;

    public void Start()
    {
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void Reset()
    {
        Stop();
        elapsedSeconds = 0.0f;
    }

    public void Restart()
    {
        Reset();
        Start();
    }

    public void Update(float deltaTime)
    {
        if (!isRunning) return;
        
        elapsedSeconds += deltaTime;

        if (IsFinished())
        {
            Stop();
            m_onFinished.Invoke();
        }
    }
}
