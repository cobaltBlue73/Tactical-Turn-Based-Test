using FSM;
using UnityEngine;

namespace GameStateSystem
{
    public abstract class GameState : StateBehaviour
    {
        protected GameStateManager gameStateManager;

        public virtual void InitializeGameStateManager(GameStateManager stateManager)
        {
            gameStateManager = stateManager;
        }
    }
}