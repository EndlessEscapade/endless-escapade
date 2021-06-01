namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceMultitileWithStyle : BasePlacementActionWithEntryAndStyle
    {
        public PlaceMultitileWithStyle(ushort entry, byte styleData) : base(entry, styleData)
        {
        }

        public override ushort Flag => 0xFFF5;

        public override void Place()
        {
        }
    }
}