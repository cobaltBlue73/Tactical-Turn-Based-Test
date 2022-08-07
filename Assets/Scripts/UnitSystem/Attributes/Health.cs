using UnityEngine;

namespace UnitSystem.Attributes
{
    public class Health : UnitAttributeGeneric<int>, IDamageable
    {
        public int MaxHealth => DefaultValue;

        public override int Value
        {
            get => base.Value;
            set
            {
                var prevValue = this.value;
                this.value = Mathf.Clamp(value, 0, MaxHealth);

                if (!prevValue.Equals(this.value))
                    ValueChangedEvent.Invoke(this.value);
            }
        }

        public void Damage(int damage)
        {
            Value -= damage;
        }
    }
}