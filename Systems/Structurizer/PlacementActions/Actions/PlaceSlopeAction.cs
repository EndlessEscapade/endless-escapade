namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceSlopeAction : BasePlacementActionWithEntryAndStyle
    {
        public PlaceSlopeAction(ushort entry, byte slope) : base(entry, slope)
        {
        }

        public override ushort Flag => 0xFFF0;

        public override void Place()
        {
        }
    }
}