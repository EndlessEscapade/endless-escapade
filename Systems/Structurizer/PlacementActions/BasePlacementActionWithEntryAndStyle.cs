namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BasePlacementActionWithEntryAndStyle : BasePlacementActionWithEntry
    {
        public override byte StyleData { get; }

        protected BasePlacementActionWithEntryAndStyle(ushort entry, byte styleData) : base(entry)
        {
            StyleData = styleData;
        }
    }
}