namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BasePlacementActionWithLiquid : BasePlacementAction, ILiquidData
    {
        public byte LiquidData { get; }

        protected BasePlacementActionWithLiquid(byte liquidData)
        {
            LiquidData = liquidData;
        }
    }
}