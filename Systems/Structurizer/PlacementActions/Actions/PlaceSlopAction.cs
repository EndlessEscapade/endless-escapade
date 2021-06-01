namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceSlopAction : BasePlacementActionWithEntryAndStyle
    {
        public PlaceSlopAction(ushort entry, byte slope) : base(entry, slope)
        {
        }

        public override ushort Flag { get; }

        public override void Place()
        {
        }
    }
}