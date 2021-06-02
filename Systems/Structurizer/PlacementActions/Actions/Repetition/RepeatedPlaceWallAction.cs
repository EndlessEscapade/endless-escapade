namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceWallAction : BaseRepeatedPlacementActionWithEntry<PlaceWallAction>
    {
        public RepeatedPlaceWallAction(ushort repetitionCount, ushort entryData) : base(repetitionCount, entryData)
        {
        }

        public override ushort Flag => 0xFFF3;

        public override PlaceWallAction PlacementAction => new PlaceWallAction(EntryData);

        public override void Place(ref int i, ref int j)
        {

        }
    }
}