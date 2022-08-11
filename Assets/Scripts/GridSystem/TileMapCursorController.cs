using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GridSystem
{
    public class TileMapCursorController : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Camera mainCamera;
        [SerializeField] private LevelMap levelMap;
        [SerializeField] private Tilemap cursorLayer;
        [SerializeField] private Tile cursorTile;

        #endregion
        
        #region Vairables

        private Vector2Int? _lastCursorCoordinates;
        private HashSet<Vector2Int> _walkablePositions;

        #endregion

        #region Methods
        
        #region Unity Callbacks

        private void Start()
        {
            _walkablePositions = levelMap.Where(cell => cell.IsWalkable)
                .Select(cell => cell.GridCoordinates).ToHashSet();
        }

        private void Update() => UpdateCursorPosition();

        #endregion

        #region Private

        private void UpdateCursorPosition()
        {
            if(!cursorLayer || !cursorTile || !levelMap) return;
            
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            var cursorCoordinates = levelMap.GetGridCoordinates(worldPos);
            
            if(!cursorCoordinates.HasValue || 
               !_walkablePositions.Contains(cursorCoordinates.Value))
            {
                if (!_lastCursorCoordinates.HasValue) return;
                
                cursorLayer.SetTile(levelMap.GetGridCell(_lastCursorCoordinates.Value).TilePosition, null);
                _lastCursorCoordinates = null;

                return;
            }

            if(_lastCursorCoordinates == cursorCoordinates) return;
            
            if(_lastCursorCoordinates.HasValue)
                cursorLayer.SetTile(levelMap.GetGridCell(_lastCursorCoordinates.Value).TilePosition, null);
            
            cursorLayer.SetTile(levelMap.GetGridCell(cursorCoordinates.Value).TilePosition, cursorTile);
            _lastCursorCoordinates = cursorCoordinates;
        }

        #endregion

        #endregion
        
       
    }
}