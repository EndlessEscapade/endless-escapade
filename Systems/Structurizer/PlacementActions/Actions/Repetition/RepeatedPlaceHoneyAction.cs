namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceHoneyAction : BaseRepeatedPlacementActionWithLiquid<PlaceHoneyAction>
    {
        public RepeatedPlaceHoneyAction(byte repetitionCount, byte liquidData) : base(repetitionCount, liquidData)
        {
        }

        public override ushort Flag => 0xFFF6;

        public override PlaceHoneyAction PlacementAction => new PlaceHoneyAction(LiquidData);
    }
}