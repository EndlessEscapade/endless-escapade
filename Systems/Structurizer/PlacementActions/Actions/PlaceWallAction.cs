namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceWallAction : BasePlacementActionWithEntry
    {
        public PlaceWallAction(ushort entry) : base(entry)
        {
        }

        public override ushort Flag { get; }

        public override void Place()
        {
        }
    }
}