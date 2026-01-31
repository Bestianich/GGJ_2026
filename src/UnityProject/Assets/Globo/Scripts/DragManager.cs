using Globo.Scripts;
using UnityEngine;

    public class DragManager : MonoBehaviour
    {
        public static DragManager Instance;
        public IDraggable CurrentDraggable { get; private set; }

        private Camera _camera;
        private void Awake()
        {
            Instance = this;
            _camera = Camera.main;
        }
    
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = Input.mousePosition;
                Ray ray = _camera.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit , Mathf.Infinity))
                {
                    CurrentDraggable = hit.collider.GetComponent<IDraggable>();
                    CurrentDraggable.EnableDragging(true);
                    Debug.Log(CurrentDraggable);
                    
                }
            }
        }
    }