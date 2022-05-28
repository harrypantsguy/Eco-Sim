using System;
using System.Collections;
using UnityEngine;

namespace _Project.Codebase
{
    public class World : MonoSingleton<World>
    {
        [field: SerializeField] public TileGrid WorldGrid { get; private set; }
        [field: SerializeField] public int WorldSize { get; private set; }

        private void Start()
        {
            WorldGrid.GenerateTileGrid(WorldSize);
            FillWorldWithTile(TileType.Dirt);
            //WorldGrid.SetGridPos(1, 1, TileType.Water);
        }

        public void FillWorldWithTile(TileType type)
        {
            StartCoroutine(SetAllTiles(type));
        }

        private IEnumerator SetAllTiles(TileType type)
        {
            int numOperationsPerFrame = 1024;

            for (int x = 0; x < WorldSize; x++)
            for (int y = 0; y < WorldSize; y++)
            {
                WorldGrid.SetGridPos(x, y, type);

                if ((x + 1) * (y + 1) % numOperationsPerFrame == 0)
                    yield return null;
            }
        }
    }
}
