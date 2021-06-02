namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BaseRepeatedPlacementActionWithEntryAndStyle<TPlacementAction> : BaseRepeatedPlacementActionWithEntry<TPlacementAction> 
        where TPlacementAction : IPlacementAction
    {
        public override byte StyleData { get; }

        protected BaseRepeatedPlacementActionWithEntryAndStyle(ushort repetitionCount, ushort entryData, byte styleData) : base(repetitionCount, entryData)
        {
            StyleData = styleData;
        }
    }
}