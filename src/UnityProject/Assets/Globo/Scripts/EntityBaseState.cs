using System;
using Globo.Scripts;
using UnityEngine;


[Serializable]
    public  class EntityBaseState
    {
        public Emotion Emotion;
        public GameObject MaskPrefab;
        public virtual void EnterState(EntityStateManager stateManager){}
        public virtual void UpdateState(EntityStateManager stateManager){}
        public virtual void OnCollisionEnter(EntityBaseState stateManager){}
    }

