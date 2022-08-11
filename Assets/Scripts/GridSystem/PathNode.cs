using System;
using UnityEngine;

namespace GridSystem
{
    public class PathNode : IGridElement, IEquatable<PathNode>
    {
        #region Constructor

        public PathNode(Vector2Int gridCoordinates, LevelMapCell levelMapCell)
        {
            GridCoordinates = gridCoordinates;
            LevelMapCell = levelMapCell;
            GCost = HCost = 0;
            PreviousNodeOnPath = null;
        }

        #endregion

        #region Properties

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => HCost + HCost;
        public Vector2Int GridCoordinates { get; }
        public LevelMapCell LevelMapCell { get; }
        public PathNode PreviousNodeOnPath { get; set; }
        public int Height => LevelMapCell.Height;
        public bool IsWalkable => LevelMapCell.IsWalkable;

        #endregion

        #region Methods

        #region Public

        public void Initialize()
        {
            GCost = int.MaxValue;
            HCost = 0;
            PreviousNodeOnPath = null;
        }

        #endregion

        #region Interfaces

        public bool Equals(IGridElement other) =>
            other is PathNode node && Equals(node);

        public bool Equals(PathNode other) =>
             other != null && 
             GridCoordinates.Equals(other.GridCoordinates);

        #endregion

        #endregion
    }
}