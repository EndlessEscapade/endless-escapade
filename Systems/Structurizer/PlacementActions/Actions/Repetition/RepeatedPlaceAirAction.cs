namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceAirAction : BaseRepeatedPlacementAction<PlaceAirAction>
    {
        public RepeatedPlaceAirAction(ushort repetitionCount) : base(repetitionCount)
        {
        }

        public override ushort Flag => 0xFFFF;

        public override PlaceAirAction PlacementAction => new PlaceAirAction();

        public override void Place(ref int i, ref int j)
        {

        }
    }
}