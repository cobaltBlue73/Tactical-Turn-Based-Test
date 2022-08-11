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

        private Vector3[] _movementPath;
        private Vector2Int[] _movementArea;
        private int _curPathIndex;

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
            
            if(!DestinationCoordinates.HasValue) return;

            var result = pathfinder.FindPath(UnitReference.GridBody.GridCoordinates,
                DestinationCoordinates.Value);

            if (!result.HasValue) return;

            _movementPath = result.Value.path.Select(coordinates =>
                levelMap.GetWorldPosition(coordinates).Value).ToArray();

            _curPathIndex = 0;
        }

        public override void OnExit()
        {
            _movementArea = null;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_movementPath is not { Length: > 0 })
            {
                OnComplete();
                return;
            }

            var targetPosition = _movementPath[_curPathIndex];

            var moveDirection = (targetPosition - cachedTransform.position).normalized;

            if (Vector3.Distance(cachedTransform.position, targetPosition) > brakingDistance)
            {
                cachedTransform.position += moveDirection * (movementSpeed * Time.deltaTime);
            }
            else
            {
                if (++_curPathIndex >= _movementPath.Length)
                    OnComplete();
            }
        }

        public IEnumerable<Vector2Int> GetActionArea() =>
            _movementArea ??= FindMovementArea();

        public bool SelectValidCoordinates(Vector2Int coordinates)
        {
            if (!GetActionArea().Contains(coordinates)) return false;

            DestinationCoordinates = coordinates;

            return true;
        }

        private Vector2Int[] FindMovementArea()
        {
            var gridBody = UnitReference.GridBody;
            var from = gridBody.GridCoordinates - Vector2Int.one * movementRange;
            var to = gridBody.GridCoordinates + Vector2Int.one * movementRange;
            var walkArea = new List<Vector2Int>();
            
            levelMap.ForEach(from, to, (x, y, cell) =>
            {
                if (cell.GridCoordinates == gridBody.GridCoordinates ||
                    cell.HasAnyGridBody()) return;

                var result = pathfinder.FindPath(gridBody.GridCoordinates, cell.GridCoordinates);

                if(!result.HasValue || 
                   result.Value.pathLength > movementRange * pathfinder.StraightMoveCost) return;
                
                walkArea.Add(cell.GridCoordinates);
            });

            return walkArea.ToArray();
        }
    }
}