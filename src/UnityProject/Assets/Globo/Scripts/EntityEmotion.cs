
using System;
using UnityEngine;

[Serializable]
    public class EntityEmotion
    {
        public GameObject MaskObject;
        public Emotion Emotion;
        public Color GizmoColor;
    }

    public enum Emotion
    {
        Rage,
        Sadness,
        Happiness,
        Disgust
    }
