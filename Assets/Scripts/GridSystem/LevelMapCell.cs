using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridSystem
{
    public class LevelMapCell : IGridElement, IEquatable<LevelMapCell>
    {
        #region Construtor

        public LevelMapCell(Vector2Int gridCoordinates, Vector3Int tilePosition, bool isWalkable)
        {
            TilePosition = tilePosition;
            IsWalkable = isWalkable;
            GridCoordinates = gridCoordinates;
            Height = tilePosition.z;
            _gridBodies = new List<GridBody>();
        }

        #endregion

        #region Properties

        public Vector3Int TilePosition { get; }
        public bool IsWalkable { get; }

        public Vector2Int GridCoordinates { get; }

        public int Height { get; }

        public IEnumerable<GridBody> GridBodies => _gridBodies;

        #endregion

        #region Variables

        private readonly List<GridBody> _gridBodies;

        #endregion

        #region Methods

        #region Public

        public void AddGridBody(GridBody gridBody) => _gridBodies.Add(gridBody);

        public void RemoveGridBody(GridBody gridBody) => _gridBodies.Remove(gridBody);

        public bool HasAnyGridBody() => _gridBodies.Any();

        public GridBody GetGridBody() => _gridBodies.FirstOrDefault();

        #endregion

        #region Interfaces

        public bool Equals(LevelMapCell other) =>
            other != null && 
            Height.Equals(other.Height) && 
            GridCoordinates.Equals(other.GridCoordinates);

        public bool Equals(IGridElement other) =>
            other is LevelMapCell data && Equals(data);

        #endregion

        #endregion
    }
}