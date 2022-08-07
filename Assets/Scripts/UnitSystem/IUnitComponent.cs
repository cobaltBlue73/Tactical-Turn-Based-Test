namespace UnitSystem
{
    public interface IUnitComponent
    {
        Unit UnitReference { get; }
        void InitializeUnitComponent(Unit unit);
    }
}