namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceWaterAction : BaseRepeatedPlacementActionWithLiquid<PlaceWaterAction>
    {
        public RepeatedPlaceWaterAction(ushort repetitionCount, byte liquidData) : base(repetitionCount, liquidData)
        {
        }

        public override ushort Flag => 0xFFF8;

        public override PlaceWaterAction PlacementAction => new PlaceWaterAction(LiquidData);

        public override void Place(ref int i, ref int j)
        {

        }
    }
}