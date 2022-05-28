using System;
using UnityEngine;

namespace _Project.Codebase
{
    public class PlayerManager : Manager
    {
        private Vector2? _worldMousePos;
        public Vector2 WorldMousePos
        {
            get
            {
                if (!_worldMousePos.HasValue)
                    _worldMousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
                return _worldMousePos.Value;
            }
        }
        private Camera _cam;

        private void Start()
        {
            _cam = Camera.main;
        }

        private void LateUpdate()
        {
            if (_worldMousePos.HasValue)
                _worldMousePos = null;
        }
    }
}