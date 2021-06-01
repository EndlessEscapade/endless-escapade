namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceHoneyAction : BasePlacementActionWithLiquid
    {
        public PlaceHoneyAction(byte liquidData) : base(liquidData)
        {
        }

        public override ushort Flag { get; }

        public override void Place()
        {
        }
    }
}