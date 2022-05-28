using CustomUI;
using UnityEngine;

namespace _Project.Codebase
{
    public class PlacementManager : Manager
    {
        public TileType placementType;
        private World _world;
        private PlayerManager _playerManager;
        private UIManager _UIManager;
        private SelectableTileItem _selectedTileItem;

        private void Start()
        {
            _world = World.Singleton;

            Managers.TryGetManager(out _playerManager);
            Managers.TryGetManager(out _UIManager);
        }

        private void Update()
        {
            if (!_UIManager.MouseInsideUI && GameControls.PlaceSelectableItem.IsHeld)
            {
                Vector2 worldMousePos = _playerManager.WorldMousePos;
                
                if (_world.WorldGrid.IsPosOnGrid(worldMousePos))
                    _world.WorldGrid.SetTileAtWorldPos(worldMousePos, placementType);
            }
        }

        public void SetPlacementType(SelectableTileItem item)
        {
            if (_selectedTileItem != null)
            {
                if (_selectedTileItem.TileType == item.TileType)
                {
                    _selectedTileItem = null;
                    placementType = TileType.Null;
                    
                    return;
                }
                
                _selectedTileItem.CancelTrigger();
            }

            _selectedTileItem = item;
            placementType = item.TileType;
        }
    }
}