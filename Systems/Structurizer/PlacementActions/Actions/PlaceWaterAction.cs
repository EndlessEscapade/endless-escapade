namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceWaterAction : BasePlacementActionWithLiquid
    {
        public PlaceWaterAction(byte liquidData) : base(liquidData)
        {
        }

        public override ushort Flag => 0xFFFB;

        public override void Place()
        {
        }
    }
}