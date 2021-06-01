namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceLavaAction : BasePlacementActionWithLiquid
    {
        public PlaceLavaAction(byte liquidData) : base(liquidData)
        {
        }

        public override ushort Flag { get; }

        public override void Place()
        {
        }
    }
}