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

        private IDamageable target;

        private Vector2Int[] attackArea;

        public override bool CanExecute =>
            base.CanExecute && target != null;

        private void Reset()
        {
            if (!gridBody)
                gridBody = GetComponent<GridBody>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            target?.Damage(attackDamage);
            OnComplete();
        }

        public override void OnExit()
        {
            attackArea = null;
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public IEnumerable<Vector2Int> GetActionArea() =>
            attackArea ??= FindAttackArea();

        public bool SelectValidCoordinates(Vector2Int coordinates)
        {
            if (!GetActionArea().Contains(coordinates)) return false;

            return levelMap.TryGetGridBodyAtGridCoordinates(coordinates, out var body) &&
                   body.TryGetComponent(out target);
        }

        private Vector2Int[] FindAttackArea()
        {
            var start = gridBody.GridCoordinates - Vector2Int.one * attackRange;
            var end = gridBody.GridCoordinates + Vector2Int.one * attackRange;

            return levelMap.From(start).To(end).Where(cell =>
            {
                if (cell.GridCoordinates == gridBody.GridCoordinates ||
                    !cell.TryGetGridBody(out var body)) return false;

                var unit = body.GetComponent<Unit>();

                return unit &&
                       unit.GetAttribute<FactionAttribute>().Value !=
                       UnitReference.GetAttribute<FactionAttribute>().Value;
            }).Select(cell => cell.GridCoordinates).ToArray();
        }
    }
}