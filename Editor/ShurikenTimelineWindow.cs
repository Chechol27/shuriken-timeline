using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class ShurikenTimelineWindow : EditorWindow
{
    private ControlData controlData;
    private List<Track> tracks = new List<Track>();
    private HorizontalResizingBar resizingBar;
    
    [SerializeField]private VisualTreeAsset windowAsset;
    [SerializeField]private VisualTreeAsset trackHeaderPanelAsset;
    [SerializeField]private VisualTreeAsset trackPanelAsset;


    private SerializedObject currentSystem;
    private TimeRulerView timeRulerView;
    
    [MenuItem("Window/Sequencing/Shuriken Timeline")]
    private static void ShowWindow()
    {
        ShurikenTimelineWindow wnd = GetWindow<ShurikenTimelineWindow>();
        wnd.titleContent = new GUIContent("Shuriken Timeline");
    }

    void CreateTrackForTransform(Transform transform ,int indentLevel, Track.TrackCreateInfo createInfo)
    {
        Track track = CreateInstance<Track>();
        Track.TrackCreateInfo newCreateInfo = createInfo;
        newCreateInfo.indentLevel = indentLevel;
        newCreateInfo.isRoot = false;
        newCreateInfo.targetSystem = transform.GetComponent<ParticleSystem>();
        track.Init(newCreateInfo);
        tracks.Add(track);
        foreach (Transform child in transform)
        {
            CreateTrackForTransform(child, indentLevel + 1, createInfo);
        }
    }
    
    void CreateTracks()
    {
        Track.TrackCreateInfo createInfo = new()
        {
            headerList = rootVisualElement.Q("pnl_header_container"),
            trackList = rootVisualElement.Q("pnl_track_container"),
            isRoot = true,
            controlData = controlData
        };
        Transform root = ((Component)currentSystem.targetObject).transform;
        CreateTrackForTransform(root, 0, createInfo);
    }

    void RemoveTracks()
    {
        foreach (Track track in tracks)
        {
            track?.Release();
        }
    }

    void SetupCurrentValue(ChangeEvent<Object> evt)
    {
        if (evt.newValue == null)
        {
            RemoveTracks();
            tracks.Clear();
            currentSystem = null;
            return;
        }
        ParticleSystem system = (ParticleSystem)evt.newValue;
        currentSystem = new SerializedObject(system);
        rootVisualElement.Bind(currentSystem);
        controlData.endTimeSeconds = ((ParticleSystem)currentSystem.targetObject).main.duration;
        CreateTracks();
        InitializeTimeRenderer();
    }

    void CreateSplitView()
    {
        var scrollView = rootVisualElement.Q<ScrollView>();
        var twoPaneSplitView = new TwoPaneSplitView
        {
            name = "pnl_split_header_tracks"
        };
        var trackHeaderPanel = trackHeaderPanelAsset.Instantiate();
        var trackPanel = trackPanelAsset.Instantiate();
        twoPaneSplitView.Add(trackHeaderPanel.contentContainer);
        twoPaneSplitView.Add(trackPanel.contentContainer);
        scrollView.Add(twoPaneSplitView);
    }

    void BindTimelineValues()
    {
        MinMaxSlider timeViewSlider = rootVisualElement.Q<MinMaxSlider>();
        timeViewSlider.RegisterValueChangedCallback(evt =>
        {
            if (currentSystem == null || currentSystem.targetObject == null || currentSystem.targetObject is not ParticleSystem ps) return;
            Vector2 newValue = evt.newValue * ps.main.duration;
            controlData.startTimeSeconds = newValue.x;
            controlData.endTimeSeconds = newValue.y;
        });
    }

    void InitializeTimeRenderer()
    {
        //timeRulerView = CreateInstance<TimeRulerView>();
        //timeRulerView.Init(rootVisualElement.Q<IMGUIContainer>("imgui_time_stamps"), rootVisualElement.Q<IMGUIContainer>(""), controlData);
    }
    
    private void CreateGUI()
    {
        Assert.IsNotNull(windowAsset);
        controlData = CreateInstance<ControlData>();
        controlData.currentTimeSeconds = 0;
        controlData.startTimeSeconds = 0;
        controlData.endTimeSeconds = 0;
        windowAsset.CloneTree(rootVisualElement);
        CreateSplitView();
        BindTimelineValues();
        rootVisualElement.Q<ObjectField>("object_target").RegisterValueChangedCallback(SetupCurrentValue);
    }
}
