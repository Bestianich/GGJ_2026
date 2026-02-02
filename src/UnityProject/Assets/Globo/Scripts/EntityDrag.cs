using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


public class EntityDrag : MonoBehaviour
{
    [SerializeField] private Transform _lastParent;
    [SerializeField] private float _sizeIncrease;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _dragDistance = 5f;
    [SerializeField] private float _offset = 5f;
    private EntityController _entity;
    private Vector3 _screenPoint;
    private Vector3 _lastScale;

    private bool _isDragging;
    private Camera _camera;
    private void Awake()
    {
        _entity = GetComponent<EntityController>();
        _camera = Camera.main;
    }


    public void Update()
    {
        if (_isDragging)
        {
            
            
            transform.position = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x , Input.mousePosition.y, _offset)); ;
            transform.up = _camera.transform.up;
            
        }
    }
    public void OnMouseDown()
    {
        
        Debug.Log("OnMouseDown");
        _entity.IsDragged = true;
        _isDragging = true;
        //_lastParent = transform.parent;
        transform.localScale *= _sizeIncrease;
        //transform.parent = _camera.transform;
        transform.parent = null;
        _entity.StopNextPoint();
        _entity.Animator.SetBool("IsGrabbed" , true);
    }

    public void Drop()
    {
        _isDragging = false;
        _entity.IsDragged = false;
        _isDragging = false;
        _entity.UpdateContinent();
        
        _entity.StartRandomPoint();
        transform.localScale /= _sizeIncrease;
       
        return;
    }

    public void OnMouseUp()
    {
        _isDragging = false;
        _entity.IsDragged = false;
        _entity.UpdateContinent();

        _entity.StartRandomPoint();
        transform.localScale /= _sizeIncrease;
        _entity.Animator.SetBool("IsGrabbed" , false);
        
    }
}
