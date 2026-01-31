using System;
using Globo.Scripts;
using UnityEngine;


    public class EntityController : MonoBehaviour 
    {
        private Vector3 _nextPoint;
        
        public bool DragIsEnabled { get; set; }

        private bool _snapped = true;
        private bool _reachedPoint = true;

        private void Update()
        {
           // transform.up = transform.position - GlobeController.Instance.GlobePivot.position;
           if(_snapped)
                SnapToSurface();
           if(_reachedPoint) 
               GenerateRandomPoint();
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
            if (transform.position == _nextPoint)
            {
                _reachedPoint = true;
                return;
            }
            transform.position = Vector3.Lerp(transform.position, _nextPoint, Time.deltaTime);
        }

        private void GenerateRandomPoint()
        {
            _nextPoint = (UnityEngine.Random.onUnitSphere * GlobeController.Instance.GlobeRadius ) - GlobeController.Instance.GlobePivot.position;
            _reachedPoint = false;
        }


        public void EnableDragging(bool enable)
        {
            DragIsEnabled = enable;
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
            DragIsEnabled = false;
            GenerateRandomPoint();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (GlobeController.Instance == null)
                return;
            Gizmos.DrawLine(GlobeController.Instance.GlobePivot.position, transform.position - GlobeController.Instance.GlobePivot.position );
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_nextPoint ,0.1f);
            Gizmos.DrawRay(transform.position , (_nextPoint - GlobeController.Instance.GlobePivot.position) -transform.position );
        }
        
    }
