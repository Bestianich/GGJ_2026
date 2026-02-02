using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(MeshCollider))]
public class Continent : MonoBehaviour
{
    [SerializeField] public List<Entity> _entities;
    [FormerlySerializedAs("_enragedEntitiesCount")] [SerializeField] public int _enragedCount;
    [SerializeField] private float _enrageInterval;
    [SerializeField] private int _enrageAmount;
    [FormerlySerializedAs("_spawnPoint")] [SerializeField] private Transform _continetCenter;
    [SerializeField] private List<Spawner> _spawners;
    [SerializeField] private float _range;
    [SerializeField] private MeshRenderer _meshRenderer;
    [FormerlySerializedAs("_GizmoColor")] [SerializeField] private Color _gizmoColor = Color.yellow;
    public MeshCollider MeshCollider;

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
                spawner.Spawn();
            }
        }
        MeshCollider = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        StartCoroutine(EnrageEntities());
    }
    
    public float FindDistance(Vector3 point)
    {
        return Mathf.Abs(Vector3.Distance(_continetCenter.position, point));
    }

    public MeshRenderer GetMeshRenderer()
    {
        return _meshRenderer;
    }

    public void AddEntity(Entity entity)
    {
        Debug.Log("Added entity: " + entity.name + " | In Continent: " + transform.name);
        _entities.Add(entity);
        entity.transform.parent = transform;
        UpdateCount();
    }

    public void RemoveEntity(Entity entity)
    {
        _entities.Remove(entity);
        UpdateCount();
    }

    public Entity NearestEntity(Vector3 point , float range)
    {
        float distance = 0f;
        Entity nearestEntity = null;
        foreach (var entity in _entities)
        {
            var temp = Vector3.Distance(point , entity.transform.position);
            if (temp > range || temp == 0)
                continue;
            if (distance <= temp)
            {
                distance = temp;
                nearestEntity = entity;
            }
        }
        return nearestEntity;
    }

    

    private IEnumerator EnrageEntities()
    {
        
        while (true)
        {
            int count = 0;
            yield return new WaitForSeconds(_enrageInterval);
            while (count < _enrageAmount && _enragedCount != _entities.Count)
            {
                var index = Random.Range(0, _entities.Count);
                if (_entities[index].GetEmotion().Emotion != Emotion.Rage)
                {
                    _entities[index].UpdateEmotion(Emotion.Rage);
                    count++;
                }

                yield return null;
            }
            UpdateCount();
        }
        yield return null;
    }

    public void UpdateCount()
    {
        _enragedCount = 0;
        foreach (var entity in _entities)
        {
            if (entity.GetEmotion().Emotion == Emotion.Rage)
                _enragedCount++;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        if(_continetCenter == null)
            return;
        Gizmos.DrawSphere(_continetCenter.position, 0.5f);
        foreach (Entity entity in _entities)
        {
            Gizmos.DrawRay(entity.transform.position, _continetCenter.position - entity.transform.position);
        }
    }
    
}
