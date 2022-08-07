using System;
using System.Linq;
using FSM;
using UnityEngine;

namespace GameStateSystem
{
    public class GameStateManager : FiniteStateMachine<GameState>
    {
        protected override void Start()
        {
            base.Start();
            CurrentState = States.FirstOrDefault();
        }

        private void Update()
        {
            if (!CurrentState) CurrentState.OnUpdate(Time.deltaTime);
        }
    }
}