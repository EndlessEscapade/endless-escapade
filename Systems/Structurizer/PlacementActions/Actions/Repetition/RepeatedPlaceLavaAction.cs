namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceLavaAction : BaseRepeatedPlacementActionWithLiquid<PlaceLavaAction>
    {
        public RepeatedPlaceLavaAction(ushort repetitionCount, byte liquidData) : base(repetitionCount, liquidData)
        {
        }

        public override ushort Flag => 0xFFF7;

        public override PlaceLavaAction PlacementAction => new PlaceLavaAction(LiquidData);

        public override void Place(ref int i, ref int j)
        {

        }
    }
}