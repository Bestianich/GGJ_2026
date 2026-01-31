using System.Collections.Generic;
using UnityEngine;


public class Continent : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private List<Entity> _entities;
    [SerializeField] private Spawner _spawner;

    private void Start()
    {
        _entities = _spawner.Spawn();
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        var entity = other.GetComponent<Entity>();
        if(entity == null)
            return;
        _entities.Add(entity);
    }

    private void OnTriggerExit(Collider other)
    {
        var entity = other.GetComponent<Entity>();
        if(entity == null)
            return;
        _entities.Remove(entity);
    }
    
}
