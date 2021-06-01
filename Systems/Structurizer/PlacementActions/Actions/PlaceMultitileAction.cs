namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceMultitileAction : BasePlacementActionWithEntry
    {
        public PlaceMultitileAction(ushort entry) : base(entry)
        {
        }

        public override ushort Flag { get; }

        public override void Place()
        {
        }
    }
}