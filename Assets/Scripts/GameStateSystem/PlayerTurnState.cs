using UnityEngine;

namespace GameStateSystem
{
    public class PlayerTurnState : FactionTurnState
    {
        public override void OnEnter()
        {
            foreach (var unit in UnitManager.Units)
                unit.UnitSelectedEvent.AddListener(HandleUnitSelected);
            base.OnEnter();
        }

        public override void OnExit()
        {
            foreach (var unit in UnitManager.Units)
                unit.UnitSelectedEvent.RemoveListener(HandleUnitSelected);
            base.OnExit();
        }

        private void HandleUnitSelected(UnitSystem.Unit unit)
        {
            UnitManager.CurrentSelectedUnit = unit;
        }
    }
}