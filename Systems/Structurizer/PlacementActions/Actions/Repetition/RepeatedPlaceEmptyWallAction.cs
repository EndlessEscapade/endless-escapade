namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceEmptyWallAction : BaseRepeatedPlacementAction<EmptyWallAction>
    {
        public RepeatedPlaceEmptyWallAction(byte repetitionCount) : base(repetitionCount)
        {
        }

        public override ushort Flag { get; }

        public override EmptyWallAction PlacementAction => new EmptyWallAction();
    }
}