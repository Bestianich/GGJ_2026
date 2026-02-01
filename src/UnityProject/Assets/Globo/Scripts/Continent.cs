using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Continent : MonoBehaviour
{
    [SerializeField] private List<Entity> _entities;
    [FormerlySerializedAs("_spawnPoint")] [SerializeField] private Transform _continetCenter;
    [SerializeField] private List<Spawner> _spawners;
    [SerializeField] private float _range;
    [SerializeField] private MeshRenderer _meshRenderer;


    private void Awake()
    {
        if(_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();
        if(_continetCenter == null)
            _continetCenter = transform;
        if (_spawners != null)
        {
            foreach (var spawner in _spawners)
            {
                _entities.AddRange(spawner.Spawn());
            }
        }
    }
    
    public float FindDistance(Vector3 point)
    {
        return Vector3.Distance(transform.position, point);
    }

    public MeshRenderer GetMeshRenderer()
    {
        return _meshRenderer;
    }

    public void AddEntity(Entity entity)
    {
        _entities.Add(entity);
    }

    public void RemoveEntity(Entity entity)
    {
        _entities.Remove(entity);
    }

    public void NearestEntity(Vector3 point)
    {
        foreach (var entity in _entities)
        {
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_continetCenter.position, 0.5f);
        foreach (Entity entity in _entities)
        {
            Gizmos.DrawRay(entity.transform.position, _continetCenter.position - entity.transform.position);
        }
    }
    
}
