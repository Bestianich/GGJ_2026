using System;
using System.Collections;
using Globo.Scripts;
using UnityEngine;
using UnityEngine.Serialization;


public class EntityController : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _waitTime;
        [SerializeField] private float _distanceRange = 2f;
        [SerializeField] private Continent _assignedContinent;
        private Transform _nextPoint;
        private Coroutine _randomPointCoroutine;

        [FormerlySerializedAs("_snapped")] public bool IsSnapped = true;
        private bool _reachedPoint = true;

        private void Start()
        {
            _assignedContinent = ContinentManager.Instance.SearchContinents(transform.position);
        }

        private void Update()
        {
           // transform.up = transform.position - GlobeController.Instance.GlobePivot.position;
           if(IsSnapped)
                SnapToSurface();
           if(_reachedPoint && _randomPointCoroutine == null) 
              _randomPointCoroutine = StartCoroutine(GenerateRandomPoint());
           if(IsSnapped || _nextPoint != null)
            Move();
        }

        private void SnapToSurface()
        {
            Vector3 direction = transform.position - GlobeController.Instance.GlobePivot.position;
            transform.position = GlobeController.Instance.GlobePivot.position + direction.normalized * GlobeController.Instance.GlobeRadius;
            transform.up = direction.normalized;
            transform.SetParent(GlobeController.Instance.GlobePivot);
        }


        private void Move()
        {
            Debug.Log(_nextPoint);
            if (Vector3.Distance(transform.position, _nextPoint.position) < 0.1f)
            {
                _reachedPoint = true;
                Destroy(_nextPoint.gameObject);
                return;
            }
            transform.position = Vector3.Lerp(transform.position, _nextPoint.transform.position, _speed * Time.deltaTime);
        }

        private IEnumerator GenerateRandomPoint()
        {
            yield return new WaitForSeconds(_waitTime);
            Vector3 point =  (UnityEngine.Random.onUnitSphere * GlobeController.Instance.GlobeRadius ) - GlobeController.Instance.GlobePivot.position;
            
            while (!_assignedContinent.GetMeshRenderer().bounds.Contains(point))
            {
                point = (UnityEngine.Random.onUnitSphere * GlobeController.Instance.GlobeRadius ) - GlobeController.Instance.GlobePivot.position;
                yield return null;
            }
            
            GameObject pointGO = new GameObject();
            pointGO.transform.position = point;
            pointGO.transform.parent = GlobeController.Instance.GlobePivot;
            _nextPoint = pointGO.transform;
            _reachedPoint = false; 
            _randomPointCoroutine = null;
            yield return null;
        }

        public void UpdateContinent()
        {
            _assignedContinent = ContinentManager.Instance.SearchContinents(transform.position);
        }

        public void StopNextPoint()
        {
            _reachedPoint = true;
            Destroy(_nextPoint.gameObject);
        }

    

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (GlobeController.Instance == null)
                return;
            Gizmos.DrawLine(GlobeController.Instance.GlobePivot.position, transform.position - GlobeController.Instance.GlobePivot.position );
            
            Gizmos.color = Color.green;
            if(_nextPoint == null)
                return;
            Gizmos.DrawSphere(_nextPoint.position ,0.1f);
            Gizmos.DrawRay(transform.position , (_nextPoint.position - GlobeController.Instance.GlobePivot.position) -transform.position );
        }
        
    }
