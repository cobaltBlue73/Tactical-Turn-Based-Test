using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GridSystem
{
    public abstract class TileMapFeedbackController : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Tilemap mapLayer;
        [SerializeField] private Tile walkableTile;

        #endregion

        #region Properties
        
        public Tilemap MapLayer => mapLayer;

        public Tile WalkableTile => walkableTile;

        #endregion

        #region Variables
        
        protected BoundsInt MapCellBounds;
        protected Camera Cam;
        protected static int? MaxHeight;
        
        #endregion

        #region Methods

        #region UNnity Callbacks

        protected virtual void Start()
        {
            Cam = Camera.main;
            MapCellBounds = MapLayer.cellBounds;

            if (MaxHeight.HasValue) return;
            
            // MaxHeight = 0;
            //
            // ForEachTile((cellPos, tile) =>
            // {
            //     if(tile != walkableTile) return;
            //
            //     if (MaxHeight < cellPos.z)
            //         MaxHeight = cellPos.z;
            // });
        }

        #endregion

        #region Protected

        protected void ForEachTile(Action<Vector3Int, Tile> onEachTile) => 
            ForEachTile(MapCellBounds.min, MapCellBounds.max, onEachTile);

        protected void ForEachTile(Vector3Int min, Vector3Int max, Action<Vector3Int, Tile> onEachTile)
        {
            for (var z = max.z; z >= min.z; --z)
            {
                for (var y = min.y; y < max.y; ++y)
                {
                    for (var x = min.x; x <max.x; ++x)
                    {
                        var tilePos = new Vector3Int(x, y, z);
                        if(!MapLayer.HasTile(tilePos)) continue;
                            
                        onEachTile?.Invoke(tilePos, MapLayer.GetTile<Tile>(tilePos));
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}