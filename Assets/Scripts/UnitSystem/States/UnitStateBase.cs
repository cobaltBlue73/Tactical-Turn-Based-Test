using FSM;
using UnityEngine;

namespace UnitSystem.States
{
    public abstract class UnitStateBase : StateBehaviour, IUnitComponent
    {
        public Unit UnitReference { get; private set; }

        public virtual void InitializeUnitComponent(Unit unit)
        {
            UnitReference = unit;
        }
    }
}