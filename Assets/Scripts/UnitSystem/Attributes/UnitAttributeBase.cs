using System;
using UnityEngine;

namespace UnitSystem.Attributes
{
    public abstract class UnitAttributeBase : MonoBehaviour, IUnitComponent
    {
        public Unit UnitReference { get; private set; }
        public void InitializeUnitComponent(Unit unit) => UnitReference = unit;
    }
}