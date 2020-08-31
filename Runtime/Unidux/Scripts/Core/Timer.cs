using UnityEngine;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System;

public class Timer
{
    public event Action OnTimerUp;

    private CancellationTokenSource _cancelSource;
    private float _time;

    public bool IsRunning { get; private set; }

    private DateTime _targetTime;
    private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

    public static Timer Create(float time, Action onTime = null)
    {
        var timer = new Timer(time);
        timer.OnTimerUp = onTime;
        return timer;
    }

    public Timer(float time)
    {
        _time = time;
    }

    public void Run()
    {
        IsRunning = true;
        _targetTime = DateTime.Now.AddSeconds(_time);
        RunAsync();
    }

    public void Stop()
    {
        if (IsRunning)
        {          
            if (_cancelSource != null)
            {
                _cancelSource.Cancel();
            }
            IsRunning = false;
        }
    }

    private async void RunAsync()
    {
        if (_cancelSource != null)
        {
            _cancelSource.Cancel();
        }
        _cancelSource = new CancellationTokenSource();
        var token = _cancelSource.Token;

        await Task.Run(() => 
        { 
            while (_targetTime > DateTime.Now) 
            { 
                if (token.IsCancellationRequested) 
                { 
                    return; 
                }
            };
            _synchronizationContext.Post((s)=>OnTimerUp?.Invoke(), null); 
        }, token);
    }
}
