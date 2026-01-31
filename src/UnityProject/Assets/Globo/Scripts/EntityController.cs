using System;
using System.Collections;
using Globo.Scripts;
using UnityEngine;


    public class EntityController : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _waitTime;
        private Transform _nextPoint;
        private Coroutine _randomPointCoroutine;

        private bool _snapped = true;
        private bool _reachedPoint = true;

        private void Update()
        {
           // transform.up = transform.position - GlobeController.Instance.GlobePivot.position;
           if(_snapped)
                SnapToSurface();
           if(_reachedPoint && _randomPointCoroutine == null) 
              _randomPointCoroutine = StartCoroutine(GenerateRandomPoint());
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
            if(_nextPoint == null)
                return;
            
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
            GameObject pointGO = new GameObject();
            pointGO.transform.position = point;
            pointGO.transform.parent = GlobeController.Instance.GlobePivot;
            _nextPoint = pointGO.transform;
            _reachedPoint = false;
            _randomPointCoroutine = null;
            yield return null;
        }

        

        public void OnMouseDown()
        {
            Debug.Log("OnMouseDown");
            _snapped = false;
            transform.SetParent(null);
        }

        public void OnMouseDrag()
        {
            Debug.Log(Input.mousePosition);
            transform.position = Input.mousePosition;
        }
        public void OnMouseUp()
        {
            _snapped = true;
            GenerateRandomPoint();
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
