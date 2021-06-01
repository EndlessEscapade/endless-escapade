namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceWaterAction : BaseRepeatedPlacementActionWithLiquid<PlaceWaterAction>
    {
        public RepeatedPlaceWaterAction(byte repetitionCount, byte liquidData) : base(repetitionCount, liquidData)
        {
        }

        public override ushort Flag { get; }

        public override PlaceWaterAction PlacementAction => new PlaceWaterAction(LiquidData);
    }
}