using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class Track : ScriptableObject
{
    public struct TrackCreateInfo
    {
        public ParticleSystem targetSystem;
        public VisualElement headerList;
        public VisualElement trackList;
        public ControlData controlData;
        public int indentLevel;
        public bool isRoot;
    }
    
    private TrackHeader header;
    private TrackStrip strip;
    private SerializedObject target;

    private List<Track> children = new List<Track>();
    public void Init(TrackCreateInfo createInfo)
    {
        target = new SerializedObject(createInfo.targetSystem);
        header = CreateInstance<TrackHeader>();
        header.Init(createInfo, target, this);
        strip = CreateInstance<TrackStrip>();
        strip.Init(createInfo, target, this);
    }

    public void Release()
    {
        header.Release();
        strip.Release();
    }
}
