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

        
        public bool IsDragged = false;
        private bool _reachedPoint = true;

        private void Start()
        {
            _assignedContinent = ContinentManager.Instance.SearchContinents(transform.position);
            _assignedContinent.AddEntity(this.GetComponent<Entity>());
        }

        private void Update()
        {
           // transform.up = transform.position - GlobeController.Instance.GlobePivot.position;
           if(!IsDragged)
                SnapToSurface();
           if(_reachedPoint && _randomPointCoroutine == null) 
              _randomPointCoroutine = StartCoroutine(GenerateRandomPoint());
           Move();
        }

        private void SnapToSurface()
        {
            Vector3 direction = transform.position - GlobeController.Instance.GlobePivot.position;
            transform.position = GlobeController.Instance.GlobePivot.position + direction.normalized * GlobeController.Instance.GlobeRadius;
            // transform.up = direction.normalized;
            if (Physics.Raycast(this.transform.position, GlobeController.Instance.GlobePivot.position - transform.position, out RaycastHit hit, Mathf.Infinity))
            {
                transform.up = hit.normal;
                transform.SetParent(_assignedContinent.transform);

            }
        }


        private void Move()
        {
            if(IsDragged || _nextPoint == null)
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
            //_assignedContinent = ContinentManager.Instance.SearchContinents(transform.position);
            if (Physics.Raycast(this.transform.position, GlobeController.Instance.GlobePivot.position - transform.position, out RaycastHit hit, Mathf.Infinity , 1 << 7))
            {
                Debug.Log(hit.transform.name);
                _assignedContinent.RemoveEntity(this.GetComponent<Entity>());
                _assignedContinent = ContinentManager.Instance.FindContinentWithMesh(hit.collider.GetComponent<MeshRenderer>());
                _assignedContinent.AddEntity(this.gameObject.GetComponent<Entity>());
                Debug.DrawRay(transform.position , -transform.up * 5f , Color.green);
            }
        }

        public void StopNextPoint()
        {
            Destroy(_nextPoint.gameObject);
            StopCoroutine(_randomPointCoroutine);
            _randomPointCoroutine = null;
            _reachedPoint = true;
        }

        public void StartRandomPoint()
        {
            _randomPointCoroutine = StartCoroutine(GenerateRandomPoint());
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
