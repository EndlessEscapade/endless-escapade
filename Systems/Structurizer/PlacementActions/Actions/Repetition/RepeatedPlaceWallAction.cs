namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceWallAction : BaseRepeatedPlacementActionWithEntry<PlaceWallAction>
    {
        public RepeatedPlaceWallAction(byte repetitionCount, ushort entryData) : base(repetitionCount, entryData)
        {
        }

        public override ushort Flag { get; }

        public override PlaceWallAction PlacementAction => new PlaceWallAction(EntryData);
    }
}