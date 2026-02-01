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
    private EntityController _entity;
    private Vector3 _screenPoint;
    private Vector3 _offset;
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
            transform.position = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _dragDistance));
            transform.up = _camera.transform.up;
        }
    }
    public void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        _entity.IsDragged = true;
        _isDragging = true;
        _lastParent = transform.parent;
        _lastScale = transform.localScale;
        
        transform.localScale *= _sizeIncrease;
        _entity.StopNextPoint();
    }
    

    public void OnMouseUp()
    {
        _entity.IsDragged = false;
        _isDragging = false;
        transform.localScale = _lastScale;
        _entity.UpdateContinent();
        _entity.StartRandomPoint();
        
    }
    
}
