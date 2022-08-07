using System.Linq;
using UnityEngine;

namespace UnitSystem.States
{
    public class AIActionSelectionState : UnitStateBase
    {
        [SerializeField] private UnitManager playerUnitManager;

        public override void OnEnter()
        {
            var attackAction = UnitReference.GetAction<AttackAction>();
            var moveAction = UnitReference.GetAction<MoveAction>();


            var sortedPlayerUnits = playerUnitManager.Units
                .OrderBy(unit => UnitReference.GridBody.DistanceTo(unit.GridBody));

            var closetsPlayerUnit = sortedPlayerUnits
                .FirstOrDefault(unit => attackAction.SelectValidCoordinates(unit.GridBody.GridCoordinates));

            if (closetsPlayerUnit)
            {
                UnitReference.SetState(attackAction);
                return;
            }

            closetsPlayerUnit = sortedPlayerUnits
                .FirstOrDefault(unit => moveAction.SelectValidCoordinates(unit.GridBody.GridCoordinates));

            if (closetsPlayerUnit)
            {
                UnitReference.SetState(moveAction);
                return;
            }

            closetsPlayerUnit = sortedPlayerUnits.First();

            var closetsGridCoordinates = moveAction.GetActionArea()
                .OrderBy(coordinates => Vector2Int.Distance(coordinates, closetsPlayerUnit.GridBody.GridCoordinates))
                .First();

            if (moveAction.SelectValidCoordinates(closetsGridCoordinates))
            {
                UnitReference.SetState(moveAction);
                return;
            }

            UnitReference.SetState<InactiveState>();
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
        }
    }
}