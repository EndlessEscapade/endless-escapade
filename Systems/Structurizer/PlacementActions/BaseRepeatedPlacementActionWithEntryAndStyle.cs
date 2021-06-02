namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BaseRepeatedPlacementActionWithEntryAndStyle : BaseRepeatedPlacementActionWithEntry
    {
        public override byte StyleData { get; }

        protected BaseRepeatedPlacementActionWithEntryAndStyle(ushort repetitionCount, ushort entryData, byte styleData)
            : base(repetitionCount, entryData)
        {
            StyleData = styleData;
        }
    }
}