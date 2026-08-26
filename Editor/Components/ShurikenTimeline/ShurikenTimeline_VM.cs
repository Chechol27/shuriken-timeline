using System;
using System.Collections.Generic;
using UnityEditor;

public class ShurikenTimeline_VM : IViewModel
{
    private float currentTimeSeconds;
    private float startTimeSeconds;
    private float endTimeSeconds;
    private SerializedObject target;

    public event Action OnNavigationStateChanged;
    public event Action OnTargetSet;

    public float TotalVisibleDuration => EndTimeSeconds - StartTimeSeconds;

    public SerializedObject Target
    {
        get => target;
        set { target = value; OnTargetSet?.Invoke();}
    }
    public float CurrentTimeSeconds
    {
        get => currentTimeSeconds;
        set { currentTimeSeconds = value;  OnNavigationStateChanged?.Invoke();}
    }

    public float StartTimeSeconds
    {
        get => startTimeSeconds;
        set { startTimeSeconds = value; OnNavigationStateChanged?.Invoke();}
    }

    public float EndTimeSeconds
    {
        get => endTimeSeconds;
        set { endTimeSeconds = value; OnNavigationStateChanged?.Invoke();}
    }

    public IViewModel Parent { get; set; }
    public List<IViewModel> Children { get; set; }
}
