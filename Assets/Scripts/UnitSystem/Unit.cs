using System;
using System.Collections.Generic;
using System.Linq;
using FSM;
using GridSystem;
using UnitSystem.Attributes;
using UnitSystem.States;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnitSystem
{
    [RequireComponent(typeof(Collider))]
    public class Unit : FiniteStateMachine<UnitStateBase>, IPointerClickHandler
    {
        #region Inspector

        [SerializeField] private UnitManager unitManager;
        [SerializeField] private GridBody gridBody;
        [SerializeField] private UnitAttributeBase[] unitAttributes;
        [SerializeField] private UnityEvent<Unit> onUnitSelected;

        #endregion

        #region Propeties

        public IEnumerable<UnitActionBase> UnitActions =>
            States.OfType<UnitActionBase>();

        public bool IsInAction => IsInState<UnitActionBase>();

        public GridBody GridBody => gridBody;

        public UnityEvent<Unit> UnitSelectedEvent => onUnitSelected;

        #endregion

        #region Variables

        private UnitActionBase selectedAction;

        #endregion

        #region Methods

        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();
            gridBody = GetComponent<GridBody>();
            unitAttributes = GetComponentsInChildren<UnitAttributeBase>();
        }

        private void Awake()
        {
            foreach (var unitComponent in GetComponentsInChildren<IUnitComponent>())
            {
                unitComponent.InitializeUnitComponent(this);
            }
        }

        private void OnEnable()
        {
            if (unitManager) unitManager.AddUnit(this);
        }

        private void OnDisable()
        {
            if (unitManager) unitManager.AddUnit(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsInState<IdleState>()) return;

            onUnitSelected.Invoke(this);
        }

        #endregion

        #region Public

        public TAttribute GetAttribute<TAttribute>() where TAttribute : UnitAttributeBase =>
            unitAttributes.FirstOrDefault(attribute => attribute is TAttribute) as TAttribute;

        public TAction GetAction<TAction>() where TAction : UnitActionBase =>
            States.FirstOrDefault(action => action is TAction) as TAction;

        public void ExecuteAction(UnitActionBase action) => SetState(action);

        public void ExecuteAction<T>() where T : UnitActionBase => SetState<T>();

        #endregion

        #endregion
    }
}