namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BaseRepeatedPlacementActionWithEntryAndStyle<TPlacementAction> : BaseRepeatedPlacementActionWithEntry<TPlacementAction> 
        where TPlacementAction : IPlacementAction
    {
        public byte StyleData { get; }

        protected BaseRepeatedPlacementActionWithEntryAndStyle(byte repetitionCount, ushort entryData, byte styleData) : base(repetitionCount, entryData)
        {
            StyleData = styleData;
        }
    }
}