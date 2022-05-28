using UnityEngine;

namespace _Project.Codebase
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _fastCamSpeed;
        [SerializeField] private float _moveLerpSpeed;
        [SerializeField] private float _zoomSpeed;
        [SerializeField] private float _zoomLerpSpeed;
        private const float MAX_ZOOM = 9f;
        private const float MIN_ZOOM = 2.5f;
        private Camera _cam;

        private Vector3 _targetPos;
        private float _targetOrthoSize;

        private void Start()
        {
            _targetPos = transform.position;

            _cam = GetComponent<Camera>();
            _targetOrthoSize = _cam.orthographicSize;
        }

        private void Update()
        {
            Vector2 inputAxis = GameControls.DirectionalInput;

            float speed = GameControls.FastCam.IsHeld ? _fastCamSpeed : _moveSpeed;

            _targetPos += (Vector3)(inputAxis * (speed * Time.deltaTime));

            transform.position = Vector3.Lerp(transform.position, _targetPos, _moveLerpSpeed * Time.deltaTime);
            
            float scrollDelta = Input.mouseScrollDelta.y;

            _targetOrthoSize -= scrollDelta * _zoomSpeed;
            _targetOrthoSize = Mathf.Clamp(_targetOrthoSize, MIN_ZOOM, MAX_ZOOM);

            _cam.orthographicSize =
                Mathf.Lerp(_cam.orthographicSize, _targetOrthoSize, _zoomLerpSpeed * Time.deltaTime);
        }
    }
}