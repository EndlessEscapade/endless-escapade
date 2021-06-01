namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceLavaAction : BaseRepeatedPlacementActionWithLiquid<PlaceLavaAction>
    {
        public RepeatedPlaceLavaAction(byte repetitionCount, byte liquidData) : base(repetitionCount, liquidData)
        {
        }

        public override ushort Flag => 0xFFF7;

        public override PlaceLavaAction PlacementAction => new PlaceLavaAction(LiquidData);
    }
}