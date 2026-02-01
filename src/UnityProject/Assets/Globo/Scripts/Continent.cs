using System.Collections.Generic;
using UnityEngine;


public class Continent : MonoBehaviour
{
    [SerializeField] private List<Entity> _entities;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _range;
    [SerializeField] private MeshRenderer _meshRenderer;




    public float FindDistance(Vector3 point)
    {
        return Vector3.Distance(transform.position, point);
    }

    public MeshRenderer GetMeshRenderer()
    {
        return _meshRenderer;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_spawnPoint.position, 0.5f);
        foreach (Entity entity in _entities)
        {
            Gizmos.DrawRay(entity.transform.position, _spawnPoint.position - entity.transform.position);
        }
    }
    
}
