using CustomUI;
using TMPro;
using UnityEngine;

namespace _Project.Codebase
{
    public class TileCursor : CustomElement
    {
        [SerializeField] private TMP_Text _tmpText;
        private Camera _cam;
        private TileGrid _world;
        private PlayerManager _playerManager;
        private Vector2 _localPos;

        private const float LERP_SPEED = 35f;
        protected override void Start()
        {
            base.Start();
            _cam = Camera.main;
            
            Managers.TryGetManager(out ReferencesManager referencesManager);
            _world = referencesManager.WorldGrid;

            Managers.TryGetManager(out _playerManager);
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            
            if (_UIManager.MouseInsideUI && !Hidden)
            {
                SetHiddenState(true);
            }
            else if (!_UIManager.MouseInsideUI && Hidden)
            {
                SetHiddenState(false);
            }

            Vector2 worldMousePos = _playerManager.WorldMousePos;

            Vector2 tileTargetPos = _world.SnapToGrid(worldMousePos);

            Vector2 targetLocalPoint = _cam.transform.InverseTransformPoint(tileTargetPos);

            _localPos = Vector2.Lerp(_localPos, targetLocalPoint, LERP_SPEED * Time.deltaTime);
            
            transform.localPosition = _localPos;
           
            if (_world.IsPosOnGrid(worldMousePos))
                _tmpText.text = _world.GetTileAtWorldPos(worldMousePos).type.ToString();
            else
                _tmpText.text = "";
        }
    }
}