namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceWaterAction : BasePlacementActionWithLiquid
    {
        public PlaceWaterAction(byte liquidData) : base(liquidData)
        {
        }

        public override ushort Flag { get; }

        public override void Place()
        {
        }
    }
}