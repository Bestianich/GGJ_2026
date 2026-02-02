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
        private Transform _nextPoint;
        private Coroutine _randomPointCoroutine;

        private Entity _entity;
        
        public bool IsDragged = false;
        private bool _reachedPoint = true;
        private int _steps = 0;
        private void Start()
        {
            _entity = GetComponent<Entity>();
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
                transform.SetParent(_entity.AssignedContinent.transform);

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
                _entity.CheckAction();
                return;
            }
            transform.position =  Vector3.MoveTowards(transform.position, _nextPoint.position, _speed * Time.deltaTime);
        }

        private IEnumerator GenerateRandomPoint()
        {
            yield return new WaitForSeconds(_waitTime);
            Vector3 point = UnityEngine.Random.onUnitSphere * GlobeController.Instance.GlobeRadius + GlobeController.Instance.GlobePivot.position;
            
            while (!IsInsideMesh(point , _entity.AssignedContinent.MeshCollider))
            {
                point =  UnityEngine.Random.onUnitSphere * GlobeController.Instance.GlobeRadius + GlobeController.Instance.GlobePivot.position;
                yield return null;
            }
            //var offset = transform.position - GlobeController.Instance.GlobePivot.position;
            //point = GlobeController.Instance.GlobePivot.position + offset.normalized * GlobeController.Instance.GlobeRadius;
            GameObject pointGO = new GameObject();
            pointGO.transform.position = point;
            pointGO.transform.parent = GlobeController.Instance.GlobePivot;
            _nextPoint = pointGO.transform;
            _reachedPoint = false; 
            _randomPointCoroutine = null;
            yield return null;
        }

        private bool IsInsideMesh(Vector3 point , MeshCollider meshCollider)
        {
            Vector3 direction = GlobeController.Instance.GlobePivot.position - point;
            Ray ray = new Ray(point, direction);
            Debug.DrawRay(point, direction * 5f , Color.magenta);
            int hitCount = 0;
            RaycastHit[] hits = Physics.RaycastAll(ray, 5f , (1 << 7) | (1 << 8));
            foreach (RaycastHit hit in hits)
            {
                if(hit.transform.CompareTag("Water"))
                    break;
                if (hit.collider == meshCollider)
                    hitCount++;
            }

            return hitCount % 2 == 1;
        }
        

        public void UpdateContinent()   
        {
            //_assignedContinent = ContinentManager.Instance.SearchContinents(transform.position);
            if (Physics.Raycast(this.transform.position, GlobeController.Instance.GlobePivot.position - transform.position, out RaycastHit hit, Mathf.Infinity , 1 << 7))
            {
                Debug.Log(hit.transform.name);
                _entity.AssignedContinent.RemoveEntity(this.GetComponent<Entity>());
                _entity.AssignedContinent.AddEntity(this.gameObject.GetComponent<Entity>());
                _entity.AssignedContinent = ContinentManager.Instance.FindContinentWithMesh(hit.collider.GetComponent<MeshRenderer>());
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
        
        

    

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            if (GlobeController.Instance == null)
                return;
            Gizmos.DrawLine(GlobeController.Instance.GlobePivot.position, transform.position - GlobeController.Instance.GlobePivot.position );
            
            Gizmos.color = Color.red;
            if(_nextPoint == null)
                return;
            Gizmos.DrawSphere(_nextPoint.position ,0.1f);
            Gizmos.DrawRay(transform.position ,   _nextPoint.position - transform.position );
        }
        
    }
