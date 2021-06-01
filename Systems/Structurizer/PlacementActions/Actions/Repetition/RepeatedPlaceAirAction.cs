namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceAirAction : BaseRepeatedPlacementAction<AirTileAction>
    {
        public RepeatedPlaceAirAction(byte repetitionCount) : base(repetitionCount)
        {
        }

        public override ushort Flag { get; }

        public override AirTileAction PlacementAction => new AirTileAction();
    }
}