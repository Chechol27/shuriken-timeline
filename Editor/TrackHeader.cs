using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TrackHeader : ScriptableObject
{
    private const float INDENT_MARGING = 25.0f;
    [SerializeField] private VisualTreeAsset trackHeaderAsset;

    private Track parentTrack;
    private VisualElement head;

    public void Init(Track.TrackCreateInfo createInfo, SerializedObject target, Track parentTrack)
    {
        this.parentTrack = parentTrack;
        head = trackHeaderAsset.Instantiate().contentContainer;
        head.Bind(target);
        head.Q<ObjectField>("object_particle_system").SetValueWithoutNotify(target.targetObject);
        head.style.marginLeft = createInfo.indentLevel * INDENT_MARGING;
        createInfo.headerList.Add(head);
    }

    public void Release()
    {
        head.parent.Remove(head);
    }
}
