using System;

public class Timer
{
    public float RemainingTime { get; private set; }
    public event Action<float> OnSecondUpdate;
    public event Action OnTimerEnd;

    public Timer(float duration)
    {
        RemainingTime = duration;
    }

    public void Tick(float deltaTime)
    {
        if (RemainingTime == 0f) return;

        RemainingTime -= deltaTime;
        OnSecondUpdate?.Invoke(RemainingTime);

        CheckTimerEnd();
    }

    private void CheckTimerEnd()
    {
        if (RemainingTime > 0f) return;

        RemainingTime = 0f;
        OnTimerEnd?.Invoke();
    }
}
