using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnitSystem.Attributes
{
    public abstract class UnitAttributeGeneric<TAttribute> : UnitAttributeBase where TAttribute : IEquatable<TAttribute>
    {
        [SerializeField] private TAttribute defaultValue;
        [SerializeField] private UnityEvent<TAttribute> onValueChanged;

        protected TAttribute value;

        public UnityEvent<TAttribute> ValueChangedEvent => onValueChanged;

        public virtual TAttribute Value
        {
            get => value;
            set
            {
                var prevValue = this.value;
                this.value = value;

                if (!prevValue.Equals(this.value))
                    onValueChanged.Invoke(this.value);
            }
        }

        public TAttribute DefaultValue => defaultValue;

        public void ResetToDefault() => Value = DefaultValue;

        private void OnEnable()
        {
            ResetToDefault();
        }
    }
}