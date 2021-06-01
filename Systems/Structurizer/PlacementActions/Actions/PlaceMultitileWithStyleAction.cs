namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceMultitileWithStyle : BasePlacementActionWithEntryAndStyle
    {
        public PlaceMultitileWithStyle(ushort entry, byte styleData) : base(entry, styleData)
        {
        }

        public override ushort Flag { get; }

        public override void Place()
        {
        }
    }
}