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
    
    [SerializeField]private VisualTreeAsset windowAsset;


    private SerializedObject currentSystem; 
    
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
            headerList = rootVisualElement.Q("pnl_track_headers"),
            trackList = rootVisualElement.Q("pnl_tracks"),
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
        //if (evt.previousValue == evt.newValue) return;
        if (evt.newValue == null)
        {
            RemoveTracks();
            tracks.Clear();
            currentSystem = null;
            return;
        }
        ParticleSystem system = (ParticleSystem)evt.newValue;
        //if (currentSystem != null && currentSystem.targetObject == system) return;
        currentSystem = new SerializedObject(system);
        rootVisualElement.Bind(currentSystem);
        controlData.endTimeSeconds = ((ParticleSystem)currentSystem.targetObject).main.duration;
        CreateTracks();
    }
    
    private void CreateGUI()
    {
        Assert.IsNotNull(windowAsset);
        controlData = CreateInstance<ControlData>();
        controlData.currentTimeSeconds = 0;
        controlData.startTimeSeconds = 0;
        controlData.endTimeSeconds = 0;
        windowAsset.CloneTree(rootVisualElement);
        rootVisualElement.Q<ObjectField>("object_target").RegisterValueChangedCallback(SetupCurrentValue);
    }
}
