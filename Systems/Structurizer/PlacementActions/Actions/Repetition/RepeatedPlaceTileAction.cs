namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceTileAction : BaseRepeatedPlacementAction<PlaceTileAction>
    {
        public ushort EntryData { get; }

        public RepeatedPlaceTileAction(byte repetitionCount, ushort entryData) : base(repetitionCount)
        {
            EntryData = entryData;
        }

        public override ushort Flag { get; }

        public override PlaceTileAction PlacementAction => new PlaceTileAction(EntryData);
    }
}