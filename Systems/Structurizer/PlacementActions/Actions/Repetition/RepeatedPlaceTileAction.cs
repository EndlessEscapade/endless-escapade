namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceTileAction : BaseRepeatedPlacementActionWithEntry<PlaceTileAction>
    {
        public RepeatedPlaceTileAction(byte repetitionCount, ushort entryData) : base(repetitionCount, entryData)
        {
        }

        public override ushort Flag => 0xFFFD;

        public override PlaceTileAction PlacementAction => new PlaceTileAction(EntryData);
    }
}