namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceHoneyAction : BaseRepeatedPlacementActionWithLiquid<PlaceHoneyAction>
    {
        public RepeatedPlaceHoneyAction(ushort repetitionCount, byte liquidData) : base(repetitionCount, liquidData)
        {
        }

        public override ushort Flag => 0xFFF6;

        public override PlaceHoneyAction PlacementAction => new PlaceHoneyAction(LiquidData);

        public override void Place(ref int i, ref int j)
        {

        }
    }
}