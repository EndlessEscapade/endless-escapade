namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceMultitileWithAlternativeStyleAction : BasePlacementActionWithEntryAndStyleAlternative
    {
        public PlaceMultitileWithAlternativeStyleAction(ushort entry, byte styleData, byte alternateStyleData) : base(entry, styleData, alternateStyleData)
        {
        }

        public override ushort Flag { get; }

        public override void Place()
        {
        }
    }
}