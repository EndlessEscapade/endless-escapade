namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BaseRepeatedPlacementActionWithLiquid<TPlacementAction> : BaseRepeatedPlacementAction<TPlacementAction> where TPlacementAction : IPlacementAction
    {
        public byte LiquidData { get; }

        protected BaseRepeatedPlacementActionWithLiquid(byte repetitionCount, byte liquidData) : base(repetitionCount)
        {
        }
    }
}