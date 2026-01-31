using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class GlobeController : MonoBehaviour
{
    public static GlobeController Instance;
    [SerializeField] private Transform _globePivot;
    [SerializeField] private float _globeRadius;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _dragForce = 1f;
    [SerializeField] private float _resetTime = 1f;
    [SerializeField] private float _resetForce = 1f;
    [SerializeField] private Rigidbody _rb;
    public Transform GlobePivot { get { return _globePivot; } }
    public float GlobeRadius { get { return _globeRadius; } }
    private Vector3 _mousePosition;
    private Vector3 _lastMousePosition;
    private Camera _mainCamera;
    private Coroutine _resetCoroutine;
    private Vector3 _point;
    
    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        Instance = this;
        _mainCamera = Camera.main;
    }
    private void Update()
    {
          var mousePosition = Input.mousePosition;
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _mousePosition = hit.point;
               // Quaternion aimRotation = Quaternion.LookRotation(_mousePosition - transform.position);
               // _pivotPosition.rotation = Quaternion.Slerp(_pivotPosition.rotation, aimRotation, Time.deltaTime * _sensibility);
                //_pivotPosition.rotation = Quaternion.Euler(0, aimRotation.eulerAngles.y * 1/_sensibility, 0f);
            }
        
    }
    
    void OnMouseDown()
    {
        //if(_resetCoroutine != null)
           // StopCoroutine(_resetCoroutine);
        _lastMousePosition = Input.mousePosition;
        _rb.angularVelocity = Vector3.zero;
    }
    
    private void OnMouseDrag()
    {
        float xRotation = Input.GetAxis("Mouse X") * _rotationSpeed;
        float yRotation = Input.GetAxis("Mouse Y") * _rotationSpeed;
        _globePivot.Rotate(Vector3.down, xRotation);
        _globePivot.Rotate(Vector3.right, yRotation);  
    }

    private void OnMouseUp()
    {
        
        Vector3 mouseDelta = Input.mousePosition - _lastMousePosition;
        Vector3 torque = new Vector3( mouseDelta.y , -mouseDelta.x , 0) * _dragForce;
        _rb.AddTorque(torque , ForceMode.Acceleration);
        _lastMousePosition = Input.mousePosition;
        //_resetCoroutine =  StartCoroutine(ResetRotation());
    }

    private IEnumerator ResetRotation()
    {
        yield return new WaitForSeconds(_resetTime);
        while (true)
        {
            _globePivot.rotation = Quaternion.Lerp(_globePivot.rotation, Quaternion.identity, _resetForce * Time.deltaTime);
            yield return null;
        }
    }

    [ContextMenu("GenerateRandomPoint")]
    public void GenerateRandomPoint()
    {
         _point = UnityEngine.Random.onUnitSphere * _globeRadius;
         Debug.Log(_point);
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        if(_mainCamera == null)
            return;
        Gizmos.DrawCube(_mainCamera.transform.position, Vector3.one);
        Gizmos.DrawRay(Camera.main.transform.position,  (_mousePosition - Camera.main.transform.position ) * 10f);
        Gizmos.DrawSphere(_point - _globePivot.position,0.1f);
    }
}
