namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceTileAction : BasePlacementActionWithEntry
    {

        public PlaceTileAction(ushort entry) : base(entry)
        {
        }

        public override ushort Flag { get; }

        public override void Place()
        {
        }
    }
}