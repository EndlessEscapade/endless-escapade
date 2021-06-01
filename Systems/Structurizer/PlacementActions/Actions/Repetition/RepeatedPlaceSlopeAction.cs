namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceSlopeAction : BaseRepeatedPlacementActionWithEntryAndStyle<PlaceSlopAction>
    {
        public RepeatedPlaceSlopeAction(byte repetitionCount, ushort entryData, byte slop) : base(repetitionCount, entryData, slop)
        {
        }

        public override ushort Flag { get; }

        public override PlaceSlopAction PlacementAction => new PlaceSlopAction(EntryData, StyleData);
    }
}