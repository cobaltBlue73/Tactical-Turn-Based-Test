using System.Collections.Generic;
using System.Linq;
using GridSystem;
using UnityEngine;

namespace UnitSystem.States
{
    [RequireComponent(typeof(GridBody))]
    public class MoveAction : UnitActionBase, IRangedAreaAction
    {
        [SerializeField] private Pathfinder pathfinder;
        [SerializeField] private LevelMap levelMap;
        [SerializeField] private int movementRange = 3;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float brakingDistance = .1f;
        [SerializeField, HideInInspector] private Transform cachedTransform;

        public int MovementRange => movementRange;

        public Vector2Int? DestinationCoordinates { get; set; }

        private Vector3[] movementPath;
        private Vector2Int[] movementArea;
        private int curPathIndex;

        public override bool CanExecute =>
            base.CanExecute && DestinationCoordinates.HasValue;

        private void Reset()
        {
            if (!cachedTransform)
                cachedTransform = transform;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            var result = pathfinder.FindPath(UnitReference.GridBody.GridCoordinates,
                DestinationCoordinates.Value);

            if (!result.HasValue) return;

            movementPath = result.Value.path.Select(coordinates =>
                levelMap.GetWorldPosition(coordinates).Value).ToArray();

            curPathIndex = 0;
        }

        public override void OnExit()
        {
            movementArea = null;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (movementPath is not { Length: > 0 })
            {
                OnComplete();
                return;
            }

            var targetPosition = movementPath[curPathIndex];

            var moveDirection = (targetPosition - cachedTransform.position).normalized;

            if (Vector3.Distance(cachedTransform.position, targetPosition) > brakingDistance)
            {
                cachedTransform.position += moveDirection * (movementSpeed * Time.deltaTime);
            }
            else
            {
                if (++curPathIndex >= movementPath.Length)
                    OnComplete();
            }
        }

        public IEnumerable<Vector2Int> GetActionArea() =>
            movementArea ??= FindMovementArea();

        public bool SelectValidCoordinates(Vector2Int coordinates)
        {
            if (!GetActionArea().Contains(coordinates)) return false;

            DestinationCoordinates = coordinates;

            return true;
        }

        private Vector2Int[] FindMovementArea()
        {
            var gridBody = UnitReference.GridBody;
            var start = gridBody.GridCoordinates - Vector2Int.one * movementRange;
            var end = gridBody.GridCoordinates + Vector2Int.one * movementRange;

            return levelMap.From(start).To(end).Where(cell =>
            {
                if (cell.GridCoordinates == gridBody.GridCoordinates ||
                    cell.HasAnyGridBody()) return false;

                var result = pathfinder.FindPath(gridBody.GridCoordinates, cell.GridCoordinates);

                return result.HasValue &&
                       result.Value.pathLength <= movementRange * pathfinder.StraightMoveCost;
            }).Select(cell => cell.GridCoordinates).ToArray();
        }
    }
}