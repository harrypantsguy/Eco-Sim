using UnityEngine;

namespace _Project.Codebase
{
    public class SelectableTileItem : SelectableItem
    {
        [field: SerializeField] public TileType TileType { get; private set; }
        private PlacementManager _placementManager;

        protected override void Start()
        {
            base.Start();

            Managers.TryGetManager(out _placementManager);
        }

        protected override void Select()
        {
            base.Select();

            _placementManager.SetPlacementType(this);
        }
    }
}