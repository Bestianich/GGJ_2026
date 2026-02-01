using System;
using System.Collections.Generic;
using UnityEngine;

    public class Entity : MonoBehaviour
    {
        [SerializeField] private List<EntityEmotion> _emotions = new List<EntityEmotion>();
        [SerializeField] private Transform _maskTransform;
        [SerializeField] private EntityEmotion _activeEmotion;


        private void Awake()
        {
            _activeEmotion = null;
            UpdateEmotion(Emotion.Happiness);
        }
        
        private void ChangeMask()
        {
            foreach (Transform child in _maskTransform)
            {
                Destroy(child.gameObject);
            } 
            Instantiate(_activeEmotion.MaskObject, _maskTransform);
            
        }

        [ContextMenu("ResetActiveEmotion")]
        private void ResetActiveEmotion()
        {
            _activeEmotion = null;
        }

        public void UpdateEmotion(Emotion emotion)
        {
            EntityEmotion temp = _activeEmotion;
            foreach (EntityEmotion em in _emotions)
            {
                if (em.Emotion == emotion)
                    temp = em;
            }
            _activeEmotion = temp;
            ChangeMask();
        }

        public EntityEmotion GetEmotion()
        {
            return _activeEmotion;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = _activeEmotion.GizmoColor;
            if(_maskTransform  == null)
                return;
            Gizmos.DrawSphere(_maskTransform.position, 0.2f);
        }
    }