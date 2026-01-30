using System;
using UnityEngine;

public class GlobeController : MonoBehaviour
{
    [SerializeField] private Transform _globePivot;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private Vector2 _zoomClamp = new Vector2(-10f, 10);
    [SerializeField] private float _zoomSpeed = 1f;
    private Vector3 _mousePosition;
    private Camera _mainCamera;
    
    private void Awake()
    {
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
            Zoom();
    }


    private void Zoom()
    {
        if(Input.GetAxis("Mouse ScrollWheel") == 0)
            return;
        _mainCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed * Time.deltaTime;
        _mainCamera.fieldOfView = Mathf.Clamp(_mainCamera.fieldOfView, _zoomClamp.x, _zoomClamp.y);
    }
    
    
    private void OnMouseDrag()
    {
        float xRotation = Input.GetAxis("Mouse X") * _rotationSpeed;
        float yRotation = Input.GetAxis("Mouse Y") * _rotationSpeed;
        _globePivot.Rotate(Vector3.down, xRotation);
        _globePivot.Rotate(Vector3.right, yRotation);  
    }


    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        if(_mainCamera == null)
            return;
        Gizmos.DrawCube(_mainCamera.transform.position, Vector3.one);
        Gizmos.DrawRay(Camera.main.transform.position,  (_mousePosition - Camera.main.transform.position ) * 10f);
    }
}
