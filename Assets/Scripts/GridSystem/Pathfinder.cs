using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = nameof(Pathfinder),
        menuName = nameof(GridSystem) + "/" + nameof(Pathfinder))]
    public class Pathfinder : ScriptableObject
    {
        #region Inspector

        [SerializeField] private LevelMap levelMap;
        [SerializeField] private int straightMoveCost = 10;
        [SerializeField] private int diagonalMoveCost = 14;
        [SerializeField] private int minStepHeight = 1;
        [SerializeField] private LayerMask obstacleLayer;

        #endregion

        #region Variable

        private Grid<PathNode> _pathNodeGrid;

        public int StraightMoveCost => straightMoveCost;

        #endregion

        #region Methods

        #region Unity Callbacks

        private void OnValidate()
        {
            diagonalMoveCost = Mathf.FloorToInt(Mathf.Sqrt(StraightMoveCost * 2));
        }

        #endregion

        #region Public

        public void Initialize()
        {
            _pathNodeGrid = new Grid<PathNode>(levelMap.GridWidth, levelMap.GridLength,
                (coordinates) => 
                    new PathNode(coordinates, levelMap.GetGridCell(coordinates)));
        }

        public PathResult? FindPath(Vector2Int startCoordinates, Vector2Int endCoordinates)
        {
            var openList = new List<PathNode>();
            var closedSet = new HashSet<PathNode>();

            foreach (var pathNode in _pathNodeGrid)
                pathNode.Initialize();

            var startNode = _pathNodeGrid.GetGridElement(startCoordinates);
            var endNode = _pathNodeGrid.GetGridElement(endCoordinates);

            startNode.GCost = 0;
            startNode.HCost = CalculateDistance(startCoordinates, endCoordinates);

            while (openList.Any())
            {
                var lowestFCostNode = openList.OrderBy(node => node.FCost).First();

                if (lowestFCostNode == endNode)
                {
                    return new PathResult
                    {
                        pathLength = endNode.FCost,
                        path = BuildPath(endNode)
                    };
                }

                openList.Remove(lowestFCostNode);
                closedSet.Add(lowestFCostNode);
                
                ForEachNeighbourNode(lowestFCostNode, (x, y, neighbourNode) =>
                {
                    if (closedSet.Contains(neighbourNode)) return;

                    var gCost = lowestFCostNode.GCost +
                                CalculateDistance(lowestFCostNode.GridCoordinates, neighbourNode.GridCoordinates);

                    if (gCost >= neighbourNode.GCost) return;

                    neighbourNode.PreviousNodeOnPath = lowestFCostNode;
                    neighbourNode.GCost = gCost;
                    neighbourNode.HCost = CalculateDistance(neighbourNode.GridCoordinates, endCoordinates);

                    if (!openList.Contains(neighbourNode))
                        openList.Add(neighbourNode);
                });
            }

            return null;
        }

        #endregion

        #region Private

        private int CalculateDistance(Vector2Int from, Vector2Int to)
        {
            var dist = from - to;
            dist.x = Mathf.Abs(dist.x);
            dist.y = Mathf.Abs(dist.y);
            var remaining = Mathf.Abs(dist.x - dist.y);
            return diagonalMoveCost * Mathf.Min(dist.x, dist.y) + StraightMoveCost * remaining;
        }

        private static IEnumerable<Vector2Int> BuildPath(PathNode endNode)
        {
            var pathNodeList = new List<PathNode> { endNode };

            for (var currentNode = endNode;
                 currentNode.PreviousNodeOnPath != null;
                 currentNode = currentNode.PreviousNodeOnPath)
            {
                pathNodeList.Add(currentNode.PreviousNodeOnPath);
            }

            return pathNodeList.Select(node => node.GridCoordinates).Reverse().ToArray();
            ;
        }

        private void ForEachNeighbourNode(PathNode currentNode, Action<int, int, PathNode> onNode)
        {
            var from = currentNode.GridCoordinates + Vector2Int.left + Vector2Int.down;
            from.x = Mathf.Max(from.x, 0);
            from.y = Mathf.Max(from.y, 0);

            var to = currentNode.GridCoordinates + Vector2Int.right + Vector2Int.up;
            to.x = Mathf.Min(to.x, _pathNodeGrid.Width - 1);
            to.y = Mathf.Min(to.y, _pathNodeGrid.Length - 1);

            _pathNodeGrid.ForEach(from, to, onNode);
        }

        #endregion

        #endregion
    }
}