namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BasePlacementActionWithEntryAndStyleAlternative : BasePlacementActionWithEntryAndStyle
    {
        public override byte AlternateStyleData { get; }

        protected BasePlacementActionWithEntryAndStyleAlternative(ushort entry, byte styleData, byte alternateStyleData)
            : base(entry, styleData)
        {
            AlternateStyleData = alternateStyleData;
        }
    }
}