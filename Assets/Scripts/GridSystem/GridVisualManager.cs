using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GridSystem
{
    public class GridVisualManager : MonoBehaviour, IGridToWorldPositionConverter, IWorldToGridCoordinatesConverter
    {
        #region Inspector

        [SerializeField] private LevelMap levelMap;
        [SerializeField] private Tilemap levelTileMap;
        [SerializeField] private List<TileBase> walkableTiles;

        #endregion

        #region Methods

        #region Unity Callbacks

        private void Awake()
        {
            if(!levelMap) return;
            
            var cellBounds = levelTileMap.cellBounds;
            
            levelMap.InitGrid(cellBounds.size.x, cellBounds.size.y, coordinates =>
            {
                var tilePosition = (Vector3Int)coordinates + cellBounds.min;
                var isWalkable = false;
                for (tilePosition.z = cellBounds.zMax; tilePosition.z >= cellBounds.zMin; --tilePosition.z)
                {
                    if (!levelTileMap.HasTile(tilePosition)) continue;

                    isWalkable = walkableTiles.Contains(levelTileMap.GetTile(tilePosition));
                    break;
                }
                
                return new LevelMapCell(coordinates, tilePosition, isWalkable);
            });
            
            levelMap.SetGridToWorldPositionConverter(this);
            levelMap.SetWorldToGridCoordinatesConverter(this);
        }

        #endregion

        #region Interfaces

        public Vector3? GetWorldPosition(int x, int y) => 
            levelMap.IsValidGridCoordinates(x, y) ? 
                levelTileMap.CellToWorld(levelMap.GetGridCell(x, y).TilePosition) :
                null;

        public Vector2Int? GetGridCoordinates(Vector3 worldPosition)
        {
            var tilePos = levelTileMap.WorldToCell(worldPosition);
            var cellCoordinates = (Vector2Int) (tilePos - levelTileMap.cellBounds.min);

            if (!levelMap.IsValidGridCoordinates(cellCoordinates)) return null;
            
            return cellCoordinates;
        }

        #endregion

        #endregion
    }
}