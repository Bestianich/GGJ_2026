using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Continent : MonoBehaviour
{
    [SerializeField] private List<Entity> _entities;
    [FormerlySerializedAs("_enragedEntitiesCount")] [SerializeField] private int _enragedCount;
    [SerializeField] private float _enrageInterval;
    [SerializeField] private int _enrageAmount;
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

    private void Start()
    {
        StartCoroutine(EnrageEntities());
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

    public Entity NearestEntity(Vector3 point , float range)
    {
        float distance = 0f;
        Entity nearestEntity = null;
        foreach (var entity in _entities)
        {
            var temp = Vector3.Distance(point , entity.transform.position);
            if (distance > temp)
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
            while (count < _enrageAmount && _enrageAmount != _enragedCount)
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
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_continetCenter.position, 0.5f);
        foreach (Entity entity in _entities)
        {
            Gizmos.DrawRay(entity.transform.position, _continetCenter.position - entity.transform.position);
        }
    }
    
}
