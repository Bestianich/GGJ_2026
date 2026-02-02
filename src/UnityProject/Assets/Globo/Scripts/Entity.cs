using System;
using System.Collections.Generic;
using UnityEngine;

    public class Entity : MonoBehaviour
    {
        [SerializeField] private List<EntityEmotion> _emotions = new List<EntityEmotion>();
        [SerializeField] private Transform _maskTransform;
        [SerializeField] private EntityEmotion _activeEmotion;
        [SerializeField] private float _rangeInteraction;
        [SerializeField] public Continent AssignedContinent;
        [SerializeField] private int _maxSteps = 3;
        private int _currentSteps = 0;


        private void Awake()
        {
            _activeEmotion = null;
            UpdateEmotion(Emotion.Happiness);
        }

        private void Start()
        {
            AssignedContinent = ContinentManager.Instance.SearchContinents(transform.position);
            AssignedContinent.AddEntity(this);
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
            if(_activeEmotion.Particle != null)
                Instantiate(_activeEmotion.Particle, _maskTransform);
        }

        public void CheckAction()
        {
            var entity = AssignedContinent.NearestEntity(transform.position, _rangeInteraction);
            switch (_activeEmotion.Emotion)
            {
                case Emotion.Rage:
                    if (entity == null)
                    {
                        _currentSteps++;
                        if(_currentSteps > _maxSteps)
                            UpdateEmotion(Emotion.Sadness);
                        break;
                    }

                    if (entity._activeEmotion.Emotion != Emotion.Rage)
                    {
                        GetComponent<EntityController>().Animator.SetTrigger("IsAngry");
                        entity.UpdateEmotion(Emotion.Rage);
                        
                    }
                    
                    break;
                case Emotion.Sadness:
                    if(entity == null)
                        break;
                    if(entity._activeEmotion.Emotion == Emotion.Happiness)
                        entity.UpdateEmotion(Emotion.Sadness);
                    break;
            }
            
        }

        public EntityEmotion GetEmotion()
        {
            return _activeEmotion;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _activeEmotion.GizmoColor;
            if(_maskTransform  == null)
                return;
            Gizmos.DrawSphere(_maskTransform.position, 0.2f);
            
            Gizmos.DrawWireSphere(transform.position, _rangeInteraction);
        }
    }