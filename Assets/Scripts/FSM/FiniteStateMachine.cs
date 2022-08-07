using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FSM
{
    public abstract class FiniteStateMachine<TState> : MonoBehaviour where TState : StateBehaviour
    {
        #region Inspector

        [SerializeField] private bool setFirstStateAsCurrentState;
        [SerializeField] private TState[] states;
        [SerializeField] private UnityEvent<TState> onStateChanged;

        #endregion

        #region Properties

        public TState CurrentState { get; protected set; }

        public UnityEvent<TState> StateChangedEvent => onStateChanged;

        public TState[] States => states;

        #endregion

        #region Variables

        #endregion

        #region Methods

        #region Unity Callbacks

        protected virtual void Reset()
        {
            states = GetComponentsInChildren<TState>();
        }

        protected virtual void Start()
        {
            if (setFirstStateAsCurrentState)
                SetState(States.FirstOrDefault());
        }

        #endregion

        #region Public

        public bool IsInState<T>() where T : TState =>
            CurrentState is T;

        public T GetState<T>() where T : TState =>
            States.FirstOrDefault(state => state is T) as T;

        public void SetState(TState nextState)
        {
            if (nextState == CurrentState) return;

            if (CurrentState) CurrentState.OnExit();

            CurrentState = nextState;

            if (CurrentState) CurrentState.OnEnter();

            onStateChanged.Invoke(CurrentState);
        }

        public void SetState<T>() where T : TState => SetState(GetState<T>());

        public virtual void OnUpdate(float deltaTime)
        {
            if (CurrentState) CurrentState.OnUpdate(deltaTime);
        }

        #endregion

        #endregion
    }
}