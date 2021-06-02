namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceTileAction : BaseRepeatedPlacementActionWithEntry<PlaceTileAction>
    {
        public RepeatedPlaceTileAction(ushort repetitionCount, ushort entryData) : base(repetitionCount, entryData)
        {
        }

        public override ushort Flag => 0xFFFD;

        public override PlaceTileAction PlacementAction => new PlaceTileAction(EntryData);

        public override void Place(ref int i, ref int j)
        {

        }
    }
}