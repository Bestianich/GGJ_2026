using System;
using System.Collections;
using Globo.Scripts;
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

    public Transform GlobePivot
    {
        get { return _globePivot; }
    }

    public float GlobeRadius
    {
        get { return _globeRadius; }
    }

    private Vector3 _mousePosition;
    private Vector3 _lastMousePosition;
    private Camera _mainCamera;
    private Vector3 _point;
    private Vector3 _mouseVelocity;

    public bool DragIsEnabled { get; set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _rb.angularVelocity = Vector3.zero;
            //Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
           _lastMousePosition = Input.mousePosition;
           float x = Input.GetAxis("Mouse X") * _rotationSpeed;
           float y = Input.GetAxis("Mouse Y") * _rotationSpeed;
           _globePivot.Rotate(Vector3.up, -x, Space.World);
           _globePivot.Rotate(Vector3.right, y, Space.World);
        }

        // if (Input.GetMouseButtonUp(1))
        // {
        //     var mouseDelta = Input.mousePosition - _lastMousePosition;
        //     Vector3 torque = new Vector3( mouseDelta.y , -mouseDelta.x , 0) * _dragForce;
        //     _rb.AddTorque(mouseDelta , ForceMode.Acceleration);
        // }
        if (Input.GetKeyDown(KeyCode.Space)) ;

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
         _point = UnityEngine.Random.onUnitSphere * GlobeRadius + GlobePivot.position;
         Debug.Log(_point);
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_point,0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_globePivot.position,GlobeRadius);
        if(_mainCamera == null)
            return;
        
        
    }
}
