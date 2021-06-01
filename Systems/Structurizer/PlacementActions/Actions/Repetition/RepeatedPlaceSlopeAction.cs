namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceSlopeAction : BaseRepeatedPlacementActionWithEntryAndStyle<PlaceSlopeAction>
    {
        public RepeatedPlaceSlopeAction(byte repetitionCount, ushort entryData, byte slop) : base(repetitionCount, entryData, slop)
        {
        }

        public override ushort Flag => 0xFFEF;

        public override PlaceSlopeAction PlacementAction => new PlaceSlopeAction(EntryData, StyleData);
    }
}