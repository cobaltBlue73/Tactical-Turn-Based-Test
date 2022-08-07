using UnitSystem.Attributes;

namespace UnitSystem.States
{
    public class DeathState : UnitStateBase
    {
        public override void InitializeUnitComponent(Unit unit)
        {
            base.InitializeUnitComponent(unit);

            var health = unit.GetAttribute<Health>();
            health.ValueChangedEvent.AddListener(healthVal =>
            {
                if (healthVal <= 0)
                    UnitReference.SetState(this);
            });
        }

        public override void OnEnter()
        {
            Destroy(UnitReference.gameObject);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
        }
    }
}