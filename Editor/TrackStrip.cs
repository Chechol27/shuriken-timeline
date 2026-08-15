using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TrackStrip : ScriptableObject
{
    private Track parentTrack;
    [SerializeField] private VisualTreeAsset trackStripAsset;
    private SerializedObject target;
    private ControlData controlData;

    private float VisibleDuration => (controlData.endTimeSeconds - controlData.startTimeSeconds);
    private ParticleSystem TargetSystem => (ParticleSystem)target.targetObject;
    
    private Vector2 Position => new(strip.resolvedStyle.width * (TargetSystem.main.startDelay.constant - controlData.startTimeSeconds / VisibleDuration), 0);

    private VisualElement strip;
    void OnGui()
    {
        target.Update();
        float fullSize = strip.resolvedStyle.width;
        float width = TargetSystem.main.duration / VisibleDuration;
        Rect barPosition = new(Position, new Vector2(width * fullSize, 100));
        GUI.Box(barPosition, TargetSystem.gameObject.name);
        //TODO: All strip logic
        //Moving and scaling affects particle system start delay and duration
        //Zoom level aware scale and positioning
    }
    
    public void Init(Track.TrackCreateInfo createInfo, SerializedObject target, Track parentTrack)
    {
        this.target = target;
        this.parentTrack = parentTrack;
        controlData = createInfo.controlData;
        strip = trackStripAsset.Instantiate().contentContainer;
        strip.Q<IMGUIContainer>().onGUIHandler += OnGui;
        createInfo.trackList.Add(strip);
    }

    public void Release()
    {
        strip.parent.Remove(strip);
    }
}
