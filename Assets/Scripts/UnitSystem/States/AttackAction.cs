using System.Collections.Generic;
using System.Linq;
using GridSystem;
using UnitSystem.Attributes;
using UnityEngine;

namespace UnitSystem.States
{
    public class AttackAction : UnitActionBase, IRangedAreaAction
    {
        [SerializeField] private LevelMap levelMap;
        [SerializeField] private GridBody gridBody;
        [SerializeField] private int attackRange = 3;
        [SerializeField] private int attackDamage = 1;

        public int AttackRange => attackRange;

        private IDamageable _target;

        private Vector2Int[] _attackArea;

        public override bool CanExecute =>
            base.CanExecute && _target != null;

        private void Reset()
        {
            if (!gridBody)
                gridBody = GetComponent<GridBody>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _target?.Damage(attackDamage);
            OnComplete();
        }

        public override void OnExit()
        {
            _attackArea = null;
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public IEnumerable<Vector2Int> GetActionArea() =>
            _attackArea ??= FindAttackArea();

        public bool SelectValidCoordinates(Vector2Int coordinates)
        {
            if (!GetActionArea().Contains(coordinates) && 
                !levelMap.AnyGridBodyAtGridCoordinates(coordinates)) return false;
            
            return levelMap.GetGridBodyAt(coordinates).TryGetComponent(out _target);
        }

        private Vector2Int[] FindAttackArea()
        {
            var from = gridBody.GridCoordinates - Vector2Int.one * attackRange;
            var to = gridBody.GridCoordinates + Vector2Int.one * attackRange;
            var attackArea = new List<Vector2Int>();

            levelMap.ForEach(from, to, (x, y, cell) =>
            {
                if (cell.GridCoordinates == gridBody.GridCoordinates) return;

                attackArea.Add(cell.GridCoordinates);
            });

            return attackArea.ToArray();
        }
    }
}