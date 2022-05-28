using _Project.Codebase;
using UnityEngine;
using static UnityEngine.Screen;

namespace CustomUI
{
    public class ScreenSpaceMarker : CustomElement
    {
        [SerializeField] private float _topPadding = 0f;
        [SerializeField] private float _bottomPadding = 0f;
        [SerializeField] private float _leftPadding = 0f;
        [SerializeField] private float _rightPadding = 0f;

        public bool TargetSet { get; private set; }
        private Transform _targetTransform;
        private Vector3 _targetLocalPos;
        public float CursorDist { get; private set; }
        public float DistToCam { get; private set; }
        public bool OnScreen { get; private set; }

        private Camera _cam;
        protected override void Start()
        {
            base.Start();
            
            _cam = Camera.main;
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            /*
            if (!Hidden && _targetTransform == null || Player.Singleton == null || Player.Singleton.ActiveCam == null)
            {
                SetHiddenState(true);
            }
            else if (Hidden)
            {
                SetHiddenState(false);
            }
            */

            if (!TargetSet) return;

            Vector3 targetPos = _targetTransform.TransformPoint(_targetLocalPos);
            
            Camera playerCam = _cam;        
            Vector3 viewportPos = playerCam.WorldToViewportPoint(targetPos);

            float zSign = Mathf.Sign(viewportPos.z);

            Vector3 relativeCenterPos = viewportPos -= Vector3.one / 2f;

            OnScreen = !Hidden && zSign > 0f;
            
            DistToCam = ((Vector2)targetPos - (Vector2)_cam.transform.position).magnitude;//(targetPos - Player.Singleton.transform.position).magnitude;
            CursorDist = OnScreen ? ((Vector2)relativeCenterPos).magnitude : Mathf.Infinity;

            float screenAngle = Mathf.Atan2(relativeCenterPos.y, relativeCenterPos.x);

            if (zSign < 0)
                screenAngle += Mathf.PI;
          
            Vector3 unitCirclePos = new Vector3(Mathf.Cos(screenAngle), Mathf.Sin(screenAngle),
                viewportPos.z);

            Vector2 screenRectTopLeft = new Vector2(-.5f, .5f) + new Vector2(_leftPadding, -_topPadding);
            Vector2 screenRectTopRight = new Vector2(.5f, .5f) + new Vector2(-_rightPadding, -_topPadding);
            Vector2 screenRectBottomRight = new Vector2(.5f, -.5f) + new Vector2(-_rightPadding, _bottomPadding);
            Vector2 screenRectBottomLeft = new Vector2(-.5f, -.5f) + new Vector2(_leftPadding, _bottomPadding);

            float unitCircleDist = CursorDist;

            if (Utils.LineSegIntersectsRect(screenRectTopLeft, screenRectTopRight,
                screenRectBottomRight, screenRectBottomLeft, Vector2.zero, unitCirclePos,
                out Vector2 intersection, true))
            {
                unitCircleDist = Mathf.Clamp(unitCircleDist, 0f, intersection.magnitude);
            }

            viewportPos = unitCirclePos * unitCircleDist + Vector3.one / 2f;
            
            RectTransform.position = viewportPos * new Vector2(width, height);
        }

        public void SetTarget(Transform obj, Vector3 position)
        {
            TargetSet = true;
            _targetTransform = obj;
            _targetLocalPos = obj.InverseTransformPoint(position);
        }
    }
}