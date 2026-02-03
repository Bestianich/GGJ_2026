using System;
using System.Collections;
using Globo.Scripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;


public class EntityController : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _waitTime;
        [SerializeField] private float _distanceRange = 2f;
        [SerializeField] public Animator Animator;
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
            Vector3 surfaceNormal = (transform.position - GlobeController.Instance.GlobePivot.position).normalized;
            transform.position = GlobeController.Instance.GlobePivot.position + surfaceNormal * GlobeController.Instance.GlobeRadius;
            transform.up = surfaceNormal;
            RotateTowardsPoint(surfaceNormal);
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
            
            Animator.SetBool("IsRunning", false);
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
            Animator.SetBool("IsRunning", true);
            yield return null;
        }

        private void RotateTowardsPoint(Vector3 surfaceNormal)
        {
            if(_nextPoint == null)
                return;
            Vector3 targetDirection = (transform.position - _nextPoint.position ).normalized;
            Vector3 projectedDirection = Vector3.ProjectOnPlane(targetDirection, surfaceNormal).normalized;

            if (projectedDirection.sqrMagnitude > 0.001f) // Check per evitare errori con vettori zero
            {
                Quaternion targetRotation = Quaternion.LookRotation(projectedDirection, surfaceNormal);
                transform.rotation = targetRotation;
            }
        }
        public bool IsInsideMesh(Vector3 point , MeshCollider meshCollider)
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
                _entity.AssignedContinent.RemoveEntity(_entity);
                _entity.AssignedContinent = ContinentManager.Instance.FindContinentWithMesh(hit.collider.GetComponent<MeshRenderer>());
                _entity.AssignedContinent.AddEntity(_entity);
                Debug.DrawRay(transform.position , -transform.up * 5f , Color.green);
            }
        }

        public void StopNextPoint()
        {
            if(_nextPoint == null || _randomPointCoroutine == null)
                return;
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
