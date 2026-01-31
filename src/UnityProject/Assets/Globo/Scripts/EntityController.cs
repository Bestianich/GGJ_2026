using UnityEngine;


    public class EntityController : MonoBehaviour
    {


        private void Update()
        {
           // transform.up = transform.position - GlobeController.Instance.GlobePivot.position;
           SnapToSurface();
        }

        private void SnapToSurface()
        {
            Vector3 direction = transform.position - GlobeController.Instance.GlobePivot.position;
            transform.position = GlobeController.Instance.GlobePivot.position + direction.normalized * GlobeController.Instance.GlobeRadius;
            transform.up = direction.normalized;
            transform.SetParent(GlobeController.Instance.GlobePivot);
            
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (GlobeController.Instance == null)
                return;
            Gizmos.DrawLine(GlobeController.Instance.GlobePivot.position, transform.position - GlobeController.Instance.GlobePivot.position );
        }
        
    }
