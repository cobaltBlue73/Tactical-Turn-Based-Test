using UnityEngine;
using UnitSystem;

namespace GameStateSystem
{
    public abstract class FactionTurnState : GameState
    {
        [SerializeField] private UnitSystem.UnitManager unitManager;

        public UnitSystem.UnitManager UnitManager => unitManager;

        public override void OnEnter()
        {
            unitManager.OnTurnEnd += OnExit;
            unitManager.StartTurn();
        }

        public override void OnExit()
        {
            unitManager.OnTurnEnd -= OnExit;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (unitManager) unitManager.OnUpdate(deltaTime);
        }
    }
}