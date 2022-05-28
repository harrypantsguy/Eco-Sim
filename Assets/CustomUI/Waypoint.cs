using System.Collections;
using System.Collections.Generic;
using _Project.Codebase;
using TMPro;
using UnityEngine;

namespace CustomUI
{
    public class Waypoint : ScreenSpaceMarker
    {
        public static List<Waypoint> waypoints = new List<Waypoint>();

        public bool OverlappingAnotherWaypoint { get; private set; }
        public bool moveText;
        public bool setTextToMaxSize;
        public bool setAlphaToMin;

        [SerializeField] private TMP_Text _textMesh;
        [SerializeField] private float _minAlpha;
        [SerializeField] private float _maxAlpha;
        [SerializeField] private float _fadeTime;
        
        private const float TEXT_MOVE_DIST = 30f;
        private const float TEXT_MOVE_SPEED = 15f;
        private const float MIN_SCALE_SIZE = .35f;
        private const float MIN_SCALE_DIST = 5f;
        private const float MAX_SCALE_DIST = 40f;
        
        private Coroutine _fadeRoutine;
        private bool _oldAlphaSetting;
        private Vector2 _defaultLocalPos;
        private Vector2 _defaultGraphicsScale;

        protected override void Awake()
        {
            base.Awake();
            
            waypoints.Add(this);
        }

        protected override void Start()
        {
            base.Start();

            _defaultLocalPos = transform.InverseTransformPoint(_textMesh.transform.position);
            _defaultGraphicsScale = GraphicsScale;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            waypoints.Remove(this);
        }
        
        protected override void LateUpdate()
        {
            base.LateUpdate();
            
            bool textInsideOtherWaypoint = false;
            
            foreach (Waypoint waypoint in waypoints)
            {
                if (waypoint == this || waypoint.Hidden) continue;
                
                bool textInOtherText = waypoint._textMesh.enabled && 
                                       _textMesh.rectTransform.Overlaps(waypoint._textMesh.rectTransform);
                bool textInOtherRect = _textMesh.rectTransform.Overlaps(waypoint.RectTransform);
                
                if (textInOtherText || textInOtherRect)
                {
                    textInsideOtherWaypoint = true;
                    break;
                }
            }

            OverlappingAnotherWaypoint = textInsideOtherWaypoint;
            
            //_textMesh.enabled = !textInsideOtherWaypoint;

            float targetScale = setTextToMaxSize
                ? 1f
                : DistToCam.ClampedRemap(MIN_SCALE_DIST, MAX_SCALE_DIST,
                    1f, MIN_SCALE_SIZE);

            GraphicsScale = _defaultGraphicsScale * targetScale;
            
            Vector2 targetPosition = _defaultLocalPos + (moveText ? new Vector2(0f, TEXT_MOVE_DIST) : Vector2.zero);

            _textMesh.transform.localPosition = Vector2.MoveTowards(_textMesh.transform.localPosition, 
                targetPosition, TEXT_MOVE_SPEED);
            
            if (_textMesh.enabled)
                _textMesh.text = Utils.ParseFloat(DistToCam, 2, 3);

            if (_oldAlphaSetting != setAlphaToMin)
            {
                if (setAlphaToMin)
                {
                    FadeAlpha(_minAlpha);
                }
                else
                {
                    FadeAlpha(_maxAlpha);
                }
            }

            _oldAlphaSetting = setAlphaToMin;
        }

        private void FadeAlpha(float alpha)
        {
            if (_fadeRoutine != null)
                StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(AlphaFadeRoutine(alpha));
        }
        
        private IEnumerator AlphaFadeRoutine(float targetAlpha)
        {
            float startAlpha = GraphicsAlpha;

            float time = 0;

            while (time < _fadeTime)
            {
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeTime);

                GraphicsAlpha = alpha;

                time += Time.deltaTime;
                yield return null;
            }
            
            _fadeRoutine = null;
        }
    }
}