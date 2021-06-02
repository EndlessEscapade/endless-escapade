namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceSlopeAction : BaseRepeatedPlacementActionWithEntryAndStyle<PlaceSlopeAction>
    {
        public RepeatedPlaceSlopeAction(ushort repetitionCount, ushort entryData, byte slope) : base(repetitionCount, entryData, slope)
        {
        }

        public override ushort Flag => 0xFFEF;

        public override PlaceSlopeAction PlacementAction => new PlaceSlopeAction(EntryData, StyleData);

        public override void Place(ref int i, ref int j)
        {

        }
    }
}