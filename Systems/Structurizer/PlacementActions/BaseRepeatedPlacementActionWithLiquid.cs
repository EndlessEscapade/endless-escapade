namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BaseRepeatedPlacementActionWithLiquid : BaseRepeatedPlacementAction, ILiquidData
    {
        public virtual byte LiquidData { get; }

        protected BaseRepeatedPlacementActionWithLiquid(ushort repetitionCount, byte liquidData) : base(repetitionCount)
        {
            LiquidData = liquidData;
        }
    }
}