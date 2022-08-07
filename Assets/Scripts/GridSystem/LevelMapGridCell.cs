using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridSystem
{
    public readonly struct LevelMapGridCell : IGridElement, IEquatable<LevelMapGridCell>
    {
        #region Construtor

        public LevelMapGridCell(Grid<LevelMapGridCell> grid, Vector2Int gridCoordinates, int height)
        {
            Grid = grid;
            GridCoordinates = gridCoordinates;
            Height = height;
            gridBodies = new List<GridBody>();
        }

        #endregion

        #region Properties

        public Grid<LevelMapGridCell> Grid { get; }

        public Vector2Int GridCoordinates { get; }

        public int Height { get; }

        public IEnumerable<GridBody> GridBodies => gridBodies;

        #endregion

        #region Variables

        private readonly List<GridBody> gridBodies;

        #endregion

        #region Methods

        #region Public

        public void AddGridBody(GridBody gridBody) =>
            gridBodies.Add(gridBody);

        public void RemoveGridBody(GridBody gridBody) =>
            gridBodies.Remove(gridBody);

        public bool HasAnyGridBody() =>
            gridBodies.Any();

        public bool TryGetGridBody(out GridBody gridBody) =>
            (gridBody = gridBodies.FirstOrDefault());

        #endregion

        #region Interfaces

        public bool Equals(LevelMapGridCell other) =>
            Grid.Equals(other.Grid) &&
            Height.Equals(other.Height) &&
            GridCoordinates.Equals(other.GridCoordinates);

        public bool Equals(IGridElement other) =>
            other is LevelMapGridCell data && Equals(data);

        #endregion

        #endregion
    }
}