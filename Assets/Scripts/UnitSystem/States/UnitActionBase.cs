using UnitSystem.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace UnitSystem.States
{
    public abstract class UnitActionBase : UnitStateBase
    {
        #region Inspector

        [SerializeField] private string displayName;
        [SerializeField] private Image displayImage;
        [SerializeField] private int actionPointCost = 1;

        #endregion

        #region Properties

        public string DisplayName => displayName;

        public int ActionPointCost => actionPointCost;

        public Image DisplayImage => displayImage;

        public bool HasEnoughActionPoints =>
            actionPoints &&
            actionPointCost <= actionPoints.Value;

        public virtual bool CanExecute =>
            HasEnoughActionPoints;

        #endregion

        #region Variables

        private ActionPoints actionPoints;

        #endregion

        #region Methods

        #region Overides

        public override void InitializeUnitComponent(Unit unit)
        {
            base.InitializeUnitComponent(unit);
            actionPoints = unit.GetAttribute<ActionPoints>();
        }

        public override void OnEnter()
        {
            actionPoints.Value -= actionPointCost;
        }

        #endregion

        #region Protected

        protected virtual void OnComplete()
        {
            if (actionPoints.Value <= 0)
                UnitReference.SetState<InactiveState>();
            else
                UnitReference.SetState<IdleState>();
        }

        #endregion

        #endregion
    }
}