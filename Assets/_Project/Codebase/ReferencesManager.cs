using UnityEngine;

namespace _Project.Codebase
{
    public class ReferencesManager : Manager
    {
        public TileGrid WorldGrid { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            WorldGrid = World.Singleton.WorldGrid;
        }
    }
}