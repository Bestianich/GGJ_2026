
using System;
using UnityEngine;

[Serializable]
    public class EntityEmotion
    {
        public GameObject MaskObject;
        public Emotion Emotion;
        public int Value;
        public Color GizmoColor;
    }

    public enum Emotion
    {
        Rage,
        Sadness,
        Happiness,
        Disgust
    }
