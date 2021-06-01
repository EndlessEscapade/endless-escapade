namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceHalfBrickAction : BasePlacementActionWithEntry
    {
        public PlaceHalfBrickAction(ushort entry) : base(entry)
        {
        }

        public override ushort Flag { get; }

        public override void Place()
        {
        }
    }
}