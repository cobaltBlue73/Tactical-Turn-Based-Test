using System;
using System.Linq;
using UnitSystem.States;
using UnityEngine;

namespace GameStateSystem
{
    public class AITurnState : FactionTurnState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            SelectNextAIAction();
        }

        private void SelectNextAIAction()
        {
            if (!UnitManager.ActiveUnits.Any()) return;

            if (!UnitManager.CurrentSelectedUnit ||
                UnitManager.CurrentSelectedUnit.IsInState<InactiveState>())
            {
                if (UnitManager.CurrentSelectedUnit)
                    UnitManager.CurrentSelectedUnit
                        .StateChangedEvent.RemoveListener(HandleSelectedUnitStateChange);

                UnitManager.CurrentSelectedUnit = UnitManager.ActiveUnits.First();
                UnitManager.CurrentSelectedUnit
                    .StateChangedEvent.AddListener(HandleSelectedUnitStateChange);
            }

            UnitManager.CurrentSelectedUnit.SetState<AIActionSelectionState>();
        }

        private void HandleSelectedUnitStateChange(UnitStateBase nextState)
        {
            switch (nextState)
            {
                case IdleState idleState:
                case InactiveState inactiveState:
                    SelectNextAIAction();
                    break;
            }
        }
    }
}