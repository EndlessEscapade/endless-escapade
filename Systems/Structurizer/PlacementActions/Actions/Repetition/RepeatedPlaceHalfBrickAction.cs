namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceHalfBrickAction : BaseRepeatedPlacementActionWithEntry<PlaceHalfBrickAction>
    {
        public RepeatedPlaceHalfBrickAction(ushort repetitionCount, ushort entryData) : base(repetitionCount, entryData)
        {
        }

        public override ushort Flag => 0xFFED;

        public override PlaceHalfBrickAction PlacementAction => new PlaceHalfBrickAction(EntryData);

        public override void Place(ref int i, ref int j)
        {

        }
    }
}