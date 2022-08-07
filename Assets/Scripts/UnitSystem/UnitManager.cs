using System;
using System.Collections.Generic;
using System.Linq;
using UnitSystem.Attributes;
using UnitSystem.States;
using UnityEngine;

namespace UnitSystem
{
    [CreateAssetMenu(fileName = nameof(UnitManager),
        menuName = nameof(UnitSystem) + "/" + nameof(UnitManager))]
    public class UnitManager : ScriptableObject
    {
        public Unit CurrentSelectedUnit
        {
            get => currentSelectedUnit;
            set
            {
                var prevUnit = currentSelectedUnit;
                currentSelectedUnit = value;

                if (prevUnit != currentSelectedUnit)
                    OnUnitSelected?.Invoke(currentSelectedUnit);
            }
        }

        public IEnumerable<Unit> Units => units;

        public IEnumerable<Unit> ActiveUnits => activeUnits;

        protected readonly List<Unit> units = new();
        protected readonly List<Unit> activeUnits = new();

        private Unit currentSelectedUnit;

        public event Action OnTurnStart;
        public event Action OnTurnEnd;
        public event Action<Unit> OnUnitSelected;

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
            unit.StateChangedEvent.AddListener(HandleUnitStateChange);
        }

        public void RemoveUnit(Unit unit)
        {
            units.Remove(unit);
            unit.StateChangedEvent.RemoveListener(HandleUnitStateChange);
        }


        private void HandleUnitStateChange(UnitStateBase unitState)
        {
            if (unitState is InactiveState)
                activeUnits.Remove(unitState.UnitReference);

            if (!ActiveUnits.Any())
                EndTurn();
        }

        public void StartTurn()
        {
            foreach (var unit in units)
            {
                unit.GetAttribute<ActionPoints>().ResetToDefault();
                unit.SetState<IdleState>();
                activeUnits.Add(unit);
            }

            OnTurnStart?.Invoke();
        }

        public void EndTurn()
        {
            foreach (var unit in units)
            {
                unit.GetAttribute<ActionPoints>().Value = 0;
                unit.SetState<InactiveState>();
            }

            activeUnits.Clear();
            OnTurnEnd?.Invoke();
        }

        public void ExecuteSelectedUnitAction(UnitActionBase action)
        {
            if (CurrentSelectedUnit)
                CurrentSelectedUnit.ExecuteAction(action);
        }

        public void ExecuteSelectedUnitAction(UnitActionBase action, Vector2Int targetCoordinates)
        {
            if (action is IRangedAreaAction rangedAction &&
                rangedAction.SelectValidCoordinates(targetCoordinates))
                ExecuteSelectedUnitAction(action);
        }

        public void OnUpdate(float deltaTime)
        {
            if (currentSelectedUnit) currentSelectedUnit.OnUpdate(deltaTime);
        }
    }
}