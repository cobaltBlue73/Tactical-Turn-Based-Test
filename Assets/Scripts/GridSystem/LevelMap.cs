using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = nameof(LevelMap),
        menuName = nameof(GridSystem) + "/" + nameof(LevelMap))]
    public class LevelMap : ScriptableObject, IEnumerable<LevelMapGridCell>
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

        private Grid<LevelMapGridCell> levelMapGrid;
        private IGridToWorldPositionConverter gridToWorldConverter;
        private IWorldToGridCoordinatesConverter worldToGridConverter;

        #endregion

        #region Methods

        #region Public

        public void InitGrid(int width, int length,
            Func<Grid<LevelMapGridCell>, Vector2Int, LevelMapGridCell> levelMapGridCellFactory)
        {
            levelMapGrid = new Grid<LevelMapGridCell>(width, length, levelMapGridCellFactory);
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

        public bool TryGetGridCell(int x, int y, out LevelMapGridCell gridCell)
        {
            gridCell = default;

            return levelMapGrid != null &&
                   levelMapGrid.TryGetGridElement(x, y, out gridCell);
        }

        public bool TryGetGridCell(Vector2Int coordinates, out LevelMapGridCell gridCell) =>
            TryGetGridCell(coordinates.x, coordinates.y, out gridCell);


        public bool IsValidGridPosition(int x, int y) =>
            levelMapGrid != null &&
            levelMapGrid.IsValidGridCoordinates(x, y);

        public bool IsValidGridPosition(Vector2Int coordinates) =>
            IsValidGridPosition(coordinates.x, coordinates.y);

        public void AddGridBodyAtGridCoordinates(int x, int y, GridBody gridBody)
        {
            if (levelMapGrid == null ||
                !levelMapGrid.TryGetGridElement(x, y, out var cell)) return;

            cell.AddGridBody(gridBody);
        }

        public void AddGridBodyAtGridCoordinates(Vector2Int coordinates, GridBody gridBody) =>
            AddGridBodyAtGridCoordinates(coordinates.x, coordinates.y, gridBody);

        public void RemoveGridBodyAtGridCoordinates(int x, int y, GridBody gridBody)
        {
            if (levelMapGrid == null ||
                !levelMapGrid.TryGetGridElement(x, y, out var cell)) return;

            cell.RemoveGridBody(gridBody);
        }

        public void RemoveGridBodyAtGridCoordinates(Vector2Int coordinates, GridBody gridBody) =>
            RemoveGridBodyAtGridCoordinates(coordinates.x, coordinates.y, gridBody);


        public bool AnyGridBodyAtGridCoordinates(int x, int y)
        {
            if (levelMapGrid == null ||
                !levelMapGrid.TryGetGridElement(x, y, out var cell))
                return false;

            return cell.HasAnyGridBody();
        }

        public bool AnyGridBodyAtGridCoordinates(Vector2Int coordinates) =>
            AnyGridBodyAtGridCoordinates(coordinates.x, coordinates.y);

        public bool TryGetGridBodyAtGridCoordinates(int x, int y, out GridBody gridBody)
        {
            gridBody = default;

            if (levelMapGrid == null ||
                !levelMapGrid.TryGetGridElement(x, y, out var cell))
                return false;

            return cell.TryGetGridBody(out gridBody);
        }

        public bool TryGetGridBodyAtGridCoordinates(Vector2Int coordinates, out GridBody gridBody) =>
            TryGetGridBodyAtGridCoordinates(coordinates.x, coordinates.y, out gridBody);

        public IEnumerable<GridBody> GetGridBodiesAtGridCoordinates(int x, int y)
        {
            if (levelMapGrid == null ||
                !levelMapGrid.TryGetGridElement(x, y, out var cell))
                return null;

            return cell.GridBodies;
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

        public LevelMap From(int x, int y)
        {
            levelMapGrid.From(x, y);
            return this;
        }

        public LevelMap From(Vector2Int coordinates) => From(coordinates.x, coordinates.y);

        public LevelMap To(int x, int y)
        {
            levelMapGrid.To(x, y);
            return this;
        }

        public IEnumerable<LevelMapGridCell> To(Vector2Int coordinates) => To(coordinates.x, coordinates.y);

        #endregion

        #region Interfaces

        public IEnumerator<LevelMapGridCell> GetEnumerator() => levelMapGrid.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #endregion
    }
}