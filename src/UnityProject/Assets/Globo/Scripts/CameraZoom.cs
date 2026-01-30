using UnityEngine;


    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private Vector2 _zoomClamp = new Vector2(-10f, 10);
        [SerializeField] private float _zoomSpeed = 1000f;
        [SerializeField] private float _currentZoom;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if(_camera == null)
                Destroy(this);
            _currentZoom = _camera.fieldOfView;
        }

        private void Update()
        {
            Zoom();
        }
        private void Zoom()
        {
            if(Input.GetAxis("Mouse ScrollWheel") == 0)
                return;
            _currentZoom -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
            _currentZoom = Mathf.Clamp(_currentZoom, _zoomClamp.x, _zoomClamp.y);
            _camera.fieldOfView = _currentZoom;
        }
    }