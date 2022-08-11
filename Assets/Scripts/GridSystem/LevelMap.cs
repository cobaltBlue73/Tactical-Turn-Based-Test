using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace GridSystem
{
    [CreateAssetMenu(fileName = nameof(LevelMap),
        menuName = nameof(GridSystem) + "/" + nameof(LevelMap))]
    public class LevelMap : ScriptableObject, IEnumerable<LevelMapCell>
    {
        #region Events

        public event Action<GridBody, Vector2Int, Vector2Int> OnGridBodyChangedCoordinates;

        #endregion

        #region Inspector

        #endregion

        #region Properties

        public int GridLength => levelMapGrid?.Length ?? 0;

        public int GridWidth => levelMapGrid?.Width ?? 0;

        #endregion

        #region Variables

        private Grid<LevelMapCell> levelMapGrid;
        private IGridToWorldPositionConverter gridToWorldConverter;
        private IWorldToGridCoordinatesConverter worldToGridConverter;

        #endregion

        #region Methods

        #region Public

        public void InitGrid(int width, int length,
            Func<Vector2Int, LevelMapCell> levelMapGridCellFactory)
        {
            levelMapGrid = new Grid<LevelMapCell>(width, length, levelMapGridCellFactory);
        }

        public void SetGridToWorldPositionConverter(IGridToWorldPositionConverter converter) =>
            gridToWorldConverter = converter;

        public Vector3? GetWorldPosition(int x, int y) =>
            gridToWorldConverter?.GetWorldPosition(x, y);

        public Vector3? GetWorldPosition(Vector2Int gridCoordinates) =>
            gridToWorldConverter?.GetWorldPosition(gridCoordinates.x, gridCoordinates.y);

        public void SetWorldToGridCoordinatesConverter(IWorldToGridCoordinatesConverter converter) =>
            worldToGridConverter = converter;

        public Vector2Int? GetGridCoordinates(Vector3 worldPosition) =>
            worldToGridConverter?.GetGridCoordinates(worldPosition);

        public LevelMapCell GetGridCell(int x, int y) => levelMapGrid.GetGridElement(x, y);

        public LevelMapCell GetGridCell(Vector2Int coordinates) => GetGridCell(coordinates.x, coordinates.y);
        
        public bool IsValidGridCoordinates(int x, int y) =>
            levelMapGrid != null &&
            levelMapGrid.IsValidGridCoordinates(x, y);

        public bool IsValidGridCoordinates(Vector2Int coordinates) =>
            IsValidGridCoordinates(coordinates.x, coordinates.y);

        public void AddGridBodyAtGridCoordinates(int x, int y, GridBody gridBody)
        {
            if (!IsValidGridCoordinates(x, y))return;

            GetGridCell(x, y).AddGridBody(gridBody);
        }

        public void AddGridBodyAtGridCoordinates(Vector2Int coordinates, GridBody gridBody) =>
            AddGridBodyAtGridCoordinates(coordinates.x, coordinates.y, gridBody);

        public void RemoveGridBodyAtGridCoordinates(int x, int y, GridBody gridBody)
        {
            if (!IsValidGridCoordinates(x, y))return;

            GetGridCell(x, y).RemoveGridBody(gridBody);
        }

        public void RemoveGridBodyAtGridCoordinates(Vector2Int coordinates, GridBody gridBody) =>
            RemoveGridBodyAtGridCoordinates(coordinates.x, coordinates.y, gridBody);


        public bool AnyGridBodyAtGridCoordinates(int x, int y)
        {
            if (levelMapGrid == null ||
                !levelMapGrid.IsValidGridCoordinates(x, y))
                return false;

            return levelMapGrid.GetGridElement(x, y).HasAnyGridBody();
        }

        public bool AnyGridBodyAtGridCoordinates(Vector2Int coordinates) =>
            AnyGridBodyAtGridCoordinates(coordinates.x, coordinates.y);

        public GridBody GetGridBodyAt(int x, int y)
        {
            if (levelMapGrid == null || !IsValidGridCoordinates(x, y))
                return null;

            return GetGridCell(x, y).GetGridBody();
        }

        public GridBody GetGridBodyAt(Vector2Int coordinates) => GetGridBodyAt(coordinates.x, coordinates.y);

        public IEnumerable<GridBody> GetGridBodiesAtGridCoordinates(int x, int y)
        {
            if (levelMapGrid == null || !IsValidGridCoordinates(x, y))
                return null;

            return GetGridCell(x, y).GridBodies;
        }

        public IEnumerable<GridBody> GetGridBodiesAtGridCoordinates(Vector2Int coordinates) =>
            GetGridBodiesAtGridCoordinates(coordinates.x, coordinates.y);

        public void UpdateGridBodyGridCoordinates(GridBody gridBody,
            Vector2Int fromCoordinates, Vector2Int toCoordinates)
        {
            RemoveGridBodyAtGridCoordinates(fromCoordinates, gridBody);
            AddGridBodyAtGridCoordinates(toCoordinates, gridBody);

            OnGridBodyChangedCoordinates?.Invoke(gridBody, fromCoordinates, toCoordinates);
        }

        public void ForEach(Action<int, int, LevelMapCell> onElement) => levelMapGrid?.ForEach(onElement);
        
        public void ForEach(Vector2Int from, Vector2Int to, Action<int, int, LevelMapCell> onElement) =>
            levelMapGrid?.ForEach(from.x, from.y, to.x, to.y, onElement);
        
        public void ForEach(int fromX, int fromY, int toX, int toY, Action<int, int, LevelMapCell> onElement) =>
            levelMapGrid?.ForEach(fromX, fromY, toX, toY, onElement);
        
        public Span<LevelMapCell> AsSpan(Vector2Int from) => levelMapGrid.AsSpan(from);

        public Span<LevelMapCell> AsSpan(Vector2Int from, Vector2Int to) => levelMapGrid.AsSpan(from, to);
        
        #endregion

        #region Interfaces

        public IEnumerator<LevelMapCell> GetEnumerator() => levelMapGrid.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #endregion
    }
}