using System;
using System.Collections.Generic;
using UnityEngine;

    public class Entity : MonoBehaviour
    {
        [SerializeField] private List<EntityEmotion> _emotions;
        [SerializeField] private Transform _maskTransform;
        [SerializeField] private EntityEmotion _activeEmotion;


        private void Awake()
        {
            _activeEmotion = null;
            ChooseEmotion();
        }

        [ContextMenu("ChooseEmotion %1")]
        private void ChooseEmotion()
        {
            Debug.Log("Choosing Emotion");   
            foreach (var emotion in _emotions )
            {
                if (_activeEmotion == null)
                {
                    _activeEmotion = emotion;
                    continue;
                }

                if (_activeEmotion.Value < emotion.Value && _activeEmotion.Emotion != emotion.Emotion)
                {
                    _activeEmotion = emotion;
                }
            }
            
            ChangeMask();
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

        private void OnDrawGizmos()
        {
            Gizmos.color = _activeEmotion.GizmoColor;
            if(_maskTransform  == null)
                return;
            Gizmos.DrawSphere(_maskTransform.position, 0.2f);
        }
    }