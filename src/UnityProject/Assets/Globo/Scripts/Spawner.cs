using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    [SerializeField] private Entity _npcPrefab;
    [SerializeField] private int _spawnAmount = 5;
    [SerializeField] private Transform _spawnPoint;
    private List<Transform> _spawnPoints;
    public bool CanSpawn = true;
    

    public void Spawn()
    {
        for (int i = 0; i < _spawnAmount; i++)
        {
            var obj = Instantiate(_npcPrefab, _spawnPoint.position, Quaternion.identity);
        }
        CanSpawn = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_spawnPoint.position, 0.1f);
    }
}
