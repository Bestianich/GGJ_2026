using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    [SerializeField] private Entity _npcPrefab;
    [SerializeField] private int _spawnAmount = 5;
    [SerializeField] private Transform _spawnPoint;
    

    public List<Entity> Spawn()
    {
        List<Entity> entities = new List<Entity>();
        for (int i = 0; i < _spawnAmount; i++)
        {
            var obj = Instantiate(_npcPrefab, _spawnPoint.position, Quaternion.identity);
            obj.transform.parent = _spawnPoint.parent;
            entities.Add(obj);
        }
        return entities;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_spawnPoint.position, 0.1f);
    }
}
