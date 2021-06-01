namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceHalfBrickAction : BaseRepeatedPlacementActionWithEntry<PlaceHalfBrickAction>
    {
        public RepeatedPlaceHalfBrickAction(byte repetitionCount, ushort entryData) : base(repetitionCount, entryData)
        {
        }

        public override ushort Flag { get; }

        public override PlaceHalfBrickAction PlacementAction => new PlaceHalfBrickAction(EntryData);
    }
}