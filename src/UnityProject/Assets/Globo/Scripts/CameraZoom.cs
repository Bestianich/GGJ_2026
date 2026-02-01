using UnityEngine;


    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private Vector2 _zoomClamp = new Vector2(0f, -100f);
        [SerializeField] private float _zoomSpeed = 1f;
        [SerializeField] private float _currentZoom;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if(_camera == null)
                Destroy(this);
            _currentZoom = _camera.transform.position.z;
        }

        private void Update()
        {
            Zoom();
        }
        private void Zoom()
        {
            if(Input.GetAxis("Mouse ScrollWheel") == 0)
                return;
            Debug.Log(Input.mouseScrollDelta.y);
            _currentZoom -= Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime;
            _currentZoom = Mathf.Clamp(_currentZoom, _zoomClamp.x, _zoomClamp.y);
            Debug.Log(_currentZoom);
            _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, _currentZoom);
        }
    }