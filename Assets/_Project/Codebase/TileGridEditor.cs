using UnityEditor;
using UnityEngine;

namespace _Project.Codebase
{
    [CustomEditor(typeof(TileGrid))]
    public class TileGridEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TileGrid tileGrid = (TileGrid)target;

            if (GUILayout.Button("Update Graphics"))
            {
                if (!Application.isPlaying) return;
                
                tileGrid.GenerateTexture();
                tileGrid.QueueGraphicUpdate();
            }
        }
    }
}