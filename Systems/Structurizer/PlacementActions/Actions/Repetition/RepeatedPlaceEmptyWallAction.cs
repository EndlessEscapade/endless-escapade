namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceEmptyWallAction : BaseRepeatedPlacementAction<PlaceEmptyWallAction>
    {
        public RepeatedPlaceEmptyWallAction(ushort repetitionCount) : base(repetitionCount)
        {
        }

        public override ushort Flag => 0xFFF1;

        public override PlaceEmptyWallAction PlacementAction => new PlaceEmptyWallAction();

        public override void Place(ref int i, ref int j)
        {

        }
    }
}