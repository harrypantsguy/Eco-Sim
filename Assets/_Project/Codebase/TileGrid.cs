using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Codebase
{
    public class TileGrid : MonoBehaviour
    {
        [SerializeField] private ComputeShader _texGeneratorCompute;
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private Texture _dirtTexture;
        [SerializeField] private Texture _waterTexture;
        [SerializeField] private Texture _stoneTexture;
        public int WorldSpaceSize { get; private set; }
        [field: SerializeField] public int PPU;
        public bool GeneratedTileGrid { get; private set; }
        
        private RenderTexture _renderTexture;
        private RectTransform _rectTransform;
        private Tile[,] _tiles;
        private ComputeBuffer _tileBuffer;
        private bool _graphicUpdateQueued = false;
        private int _imageSize;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            _rawImage.color = Color.clear;
        }

        public void GenerateTileGrid(int WorldSpaceSize)
        {
            GeneratedTileGrid = true;
            _rawImage.color = Color.white;
            this.WorldSpaceSize = WorldSpaceSize;
            
            _tileBuffer = new ComputeBuffer(WorldSpaceSize * WorldSpaceSize, 4);
            _tiles = new Tile[WorldSpaceSize, WorldSpaceSize];
            
            _rectTransform.sizeDelta = new Vector2(WorldSpaceSize, WorldSpaceSize);

            GenerateTexture();
        }

        public void GenerateTexture()
        {
            _imageSize = WorldSpaceSize * PPU;

            _renderTexture = new RenderTexture(_imageSize, _imageSize, 32)
            {
                enableRandomWrite = true,
                useMipMap = false,
                filterMode = FilterMode.Point
            };

            _renderTexture.Create();
            _rawImage.texture = _renderTexture;

            QueueGraphicUpdate();
        }

        private void LateUpdate()
        {
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = -Vector2Int.one;
            if (IsPosOnGrid(worldMousePos))
            {
                gridPos = WorldToTilePos(worldMousePos);
            }

            if (_graphicUpdateQueued)
            {
                _graphicUpdateQueued = false;
                UpdateGraphics();
            }
        }

        private void UpdateGraphics()
        {
            _texGeneratorCompute.SetTexture(0, "DirtTexture", _dirtTexture);
            _texGeneratorCompute.SetTexture(0, "WaterTexture", _waterTexture);
            _texGeneratorCompute.SetTexture(0, "StoneTexture", _stoneTexture);
            
            _texGeneratorCompute.SetInt("WorldSpaceSize", WorldSpaceSize);
            _texGeneratorCompute.SetInt("PPU", PPU);
            _texGeneratorCompute.SetTexture(0, "Result", _renderTexture);
            
            _tileBuffer.SetData(_tiles);
            _texGeneratorCompute.SetBuffer(0, "Tiles", _tileBuffer);
            
            _texGeneratorCompute.Dispatch(0, _renderTexture.width / 8,
                _renderTexture.height / 8, 1);
        }

        private void OnDestroy()
        {
            _tileBuffer.Release();
        }

        public void QueueGraphicUpdate()
        {
            _graphicUpdateQueued = true;
        }

        public bool IsPosOnGrid(Vector2 pos)
        {
            Vector2Int tilePos = WorldToTilePos(pos);
            return tilePos.x >= 0 && tilePos.y >= 0 && tilePos.x < WorldSpaceSize && tilePos.y < WorldSpaceSize;
        }

        public Vector2Int WorldToTilePos(Vector2 pos)
        {
            Vector3 position = _rectTransform.position;
            return new Vector2Int(Mathf.FloorToInt(pos.x - position.x),
                Mathf.FloorToInt(pos.y - position.y));
        }

        public Vector2 SnapToGrid(Vector2 pos)
        {
            return WorldToTilePos(pos) + (Vector2)transform.position + Vector2.one/2f;        
        }

        public void SetGridPos(Vector2Int pos, TileType type)
        {
            _tiles[pos.x, pos.y].type = type;
            QueueGraphicUpdate();
        }
        
        public void SetGridPos(int x, int y, TileType type)
        {
            SetGridPos(new Vector2Int(x, y), type);
        }
        
        public void SetTileAtWorldPos(Vector2 pos, TileType type)
        {
            SetGridPos(WorldToTilePos(pos), type);
        }
        
        public void SetTileAtWorldPos(float x, float y, TileType type)
        {
            SetTileAtWorldPos(new Vector2(x, y), type);
        }


        public Tile GetTileAtWorldPos(Vector2 pos)
        {
            Vector2Int tilePos = WorldToTilePos(pos);
            return _tiles[tilePos.x, tilePos.y];
        }
        
        public Tile GetTileAtWorldPos(float x, float y)
        {
            return GetTileAtWorldPos(new Vector2(x, y));
        }
    }
}