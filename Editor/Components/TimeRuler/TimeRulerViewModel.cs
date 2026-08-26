using System;

public class TimeRulerViewModel
{
    private readonly ShurikenTimeline_VM rootVm;
    public float CurrentTimeSeconds => rootVm.CurrentTimeSeconds;
    public float StartTimeSeconds => rootVm.StartTimeSeconds;
    public float EndTimeSeconds => rootVm.EndTimeSeconds;
    
    public event Action OnStateChanged;

    public TimeRulerViewModel(ShurikenTimeline_VM rootVm)
    {
        this.rootVm = rootVm;
        this.rootVm.OnNavigationStateChanged += () => OnStateChanged?.Invoke();
    }
}
